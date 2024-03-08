using AutoMapper;
using Evoltis.Entity;
using Evoltis.Helpers;
using Evoltis.Models;
using Evoltis.Models.Dtos.ClubDtos;
using Evoltis.Repositories.Interfaces;
using Evoltis.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evoltis.Services
{
    public class ClubService: IClubService
    {
        private readonly IClubRepository iClubRepository;
        private readonly IDbOperation iDbOperation;
        private readonly IHelperFile iHelperFile;
        private readonly IMapper iMapper;
        private readonly ITournamentRepository iTournamentRepository;

        public ClubService(IClubRepository iClubRepository, 
            IDbOperation iDbOperation, 
            IHelperFile iHelperFile,
            IMapper iMapper,
            ITournamentRepository iTournamentRepository)
        {
            this.iClubRepository = iClubRepository;
            this.iDbOperation = iDbOperation;
            this.iHelperFile = iHelperFile;
            this.iMapper = iMapper;
            this.iTournamentRepository = iTournamentRepository;
        }

        public async Task<ResponseObjectJsonDto> DisableClub(int idClub)
        {
            try
            {
                Club club = await iClubRepository.GetClub(idClub);

                if(club.Active == false)
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodeHTTP.BADREQUEST,
                        Message = "El club ya fue dado de baja"
                    };
                }

                club.Active = false;
                iClubRepository.UpdateClub(club);

                if (!await iDbOperation.Save())
                {
                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = "Error al desactivar el club." };
                }
                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodeHTTP.OK,
                    Message = "Club desactivado con éxito"
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Response = ex };
            }
        }

        public async Task<ResponseObjectJsonDto> CreateClub(ClubCreateDto clubDto)
        {

            using (IDbContextTransaction dbContextTransaction = await iDbOperation.BeginTransaction())
            {
                try
                {

                    string fileName = "";

                    if (await iClubRepository.ClubExistName(clubDto.Name))
                    {
                        await iDbOperation.Rollback(dbContextTransaction);
                        return new ResponseObjectJsonDto { Code = (int)CodeHTTP.BADREQUEST, Message = "Ya existe un club registrado con ese Nombre" };
                    }

                    if (await iClubRepository.ClubExistCuit(clubDto.CUIT))
                    {
                        return new ResponseObjectJsonDto { Code = (int)CodeHTTP.BADREQUEST, Message = "Ya existe un club registrado con ese CUIT" };
                    }

                    if (clubDto.ImageLogo != null)
                    {
                        string extension = Path.GetExtension(clubDto.ImageLogo.FileName).ToLower();

                        Guid guid = Guid.NewGuid();
                        fileName = $"{guid.ToString()}{extension}";
                        bool result = await iHelperFile.Upload(iHelperFile.GetPathClubImage(), fileName, clubDto.ImageLogo);

                        if (!result)
                        {
                            await iDbOperation.Rollback(dbContextTransaction);
                            return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = "Error al cargar la imagen" };
                        }
                    }

                    Club club = iMapper.Map<ClubCreateDto, Club>(clubDto);
                    club.FileName = fileName;
                    club.Active = true;

                    await iClubRepository.CreateClub(club);

                    if (!await iDbOperation.Save())
                    {
                        return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = "Error de servidor al crear el club." };
                    }

                    await iDbOperation.Commit(dbContextTransaction);
                }
                catch (Exception ex)
                {
                    await iDbOperation.Rollback(dbContextTransaction);
                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Response = ex };
                }

            }
            return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.OK, Message = "Club creado exitosamente" };
        }

        public async Task<ResponseObjectJsonDto> GetClubsByFilters(ClubFiltersDto filters)
        {
            try
            {

                IList<Club> lstClubs = lstClubs = await iClubRepository.GetClubsByFilters(filters);
                IList<ClubGetDto> lstClubsGetDto = new List<ClubGetDto>();

                if (lstClubs == null || lstClubs.Count == 0)
                {
                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.OK, Message = "La consulta no brindó resultados" };
                }
                else
                {
                    lstClubsGetDto = iMapper.Map<IList<ClubGetDto>>(lstClubs);
                }

                decimal numberOfPages = 0;

                if (filters.Pagination == null)
                {
                    filters.Pagination = new();
                    filters.Pagination.IsPaginated = false;
                }

                if (filters.Pagination.IsPaginated)
                {
                    var result = lstClubsGetDto.Page(filters.Pagination);

                    numberOfPages = Math.Ceiling((decimal)lstClubsGetDto.Count / (decimal)filters.Pagination.AmountRegistersPage);

                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodeHTTP.OK,
                        Response = new
                        {
                            TotalQuantity = lstClubsGetDto.Count,
                            numberOfPages,
                            lstClubsGetDto = result
                        }
                    };
                }
                else
                {
                    return new ResponseObjectJsonDto()
                    {
                        Code = (int)CodeHTTP.OK,
                        Response = new
                        {
                            TotalQuantity = lstClubsGetDto.Count,
                            numberOfPages,
                            lstClubsGetDto
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Response = ex };
            }
        }

        public async Task<ResponseObjectJsonDto> GetClubById(int idClub)
        {
            try
            {
                Club club = await iClubRepository.GetClub(idClub);

                ClubUpdateDto clubUpdateDto = new ClubUpdateDto();

                if (club == null)
                {
                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.OK, Message = "La consulta no brindó resultados" };
                }
                else
                {
                    clubUpdateDto = iMapper.Map<ClubUpdateDto>(club);
                }

                return new ResponseObjectJsonDto()
                {
                    Code = (int)CodeHTTP.OK,
                    Response = clubUpdateDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Response = ex };
            }
        }

        public async Task<FileStream> GetImageClubById(int idClub)
        {

            Club club = await iClubRepository.GetClub(idClub);

            if (club == null)
            {
                return null;
            }

            FileStream file = iHelperFile.GetFile(iHelperFile.GetPathClubImage(), club.FileName);

            if (file == null)
            {
                return null;
            }

            return file;
        }

        public async Task<ResponseObjectJsonDto> UpdateClub(ClubPatchDto clubDto)
        {

            if (await iClubRepository.ClubUpdateExistsName(clubDto.IdClub, clubDto.Name))
            {
                return new ResponseObjectJsonDto { Code = (int)CodeHTTP.BADREQUEST, 
                    Message = "Ya existe un club registrado con el mismo nombre." };
            }

            Club club = iMapper.Map<ClubPatchDto, Club>(clubDto);
            string fileName = "";

            using (IDbContextTransaction dbContextTransaction = await iDbOperation.BeginTransaction())
            {
                try
                {
                    if (await iClubRepository.ClubHaveImage(clubDto.IdClub))
                    {
                        Club current = await iClubRepository.GetClub(clubDto.IdClub);
                        string currentFileName = current.FileName;
                        if (clubDto.ImageLogo != null)
                        {
                            byte[] currentImage = iHelperFile.ConvertFileToArrayByte(iHelperFile.GetPathClubImage(), currentFileName);
                            byte[] newImage = iHelperFile.ConvertIFormFileToArrayByte(clubDto.ImageLogo);
                            //VALIDA SI LA IMAGEN SUBIDA ES IGUAL A LA ANTERIOR
                            if (!newImage.SequenceEqual(currentImage))
                            {
                                //GENERA FILENAME DE IMAGEN
                                fileName = iHelperFile.GetFileName(clubDto.ImageLogo);

                                //REEMPLAZA IMAGEN NUEVA POR LA ACTUAL
                                bool result = iHelperFile.ReplaceFileDecrypt(iHelperFile.GetPathClubImage(), currentFileName, fileName, newImage);

                                if (!result)
                                {
                                    await iDbOperation.Rollback(dbContextTransaction);
                                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = "Error al guardar la nueva imagen." };
                                }
                            }
                            else
                            {
                                fileName = currentFileName;
                            }
                        }
                        else
                        {
                            fileName = currentFileName;
                        }
                    }
                    else
                    {
                        if (clubDto.ImageLogo != null)
                        {
                            IFormFile image = clubDto.ImageLogo;
                            fileName = iHelperFile.GetFileName(image);
                            if (!await iHelperFile.Upload(iHelperFile.GetPathClubImage(), fileName, image))
                            {
                                await iDbOperation.Rollback(dbContextTransaction);
                                return new ResponseObjectJsonDto { Code = (int)CodeHTTP.CONFLICT, Message = "Error al guardar la imagen." };
                            }
                        }
                    }

                    club.FileName = fileName;

                    iClubRepository.UpdateClub(club);

                    if (!await iDbOperation.Save())
                    {
                        await iDbOperation.Rollback(dbContextTransaction);
                        return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = "Error al modificar el club." };
                    }
                }
                catch (Exception ex)
                {
                    await iDbOperation.Rollback(dbContextTransaction);
                    return new ResponseObjectJsonDto() { Code = (int)CodeHTTP.INTERNALSERVER, Message = ex.ToString() };
                }

                await iDbOperation.Commit(dbContextTransaction);
            }

            return new ResponseObjectJsonDto { Code = (int)CodeHTTP.OK, Message = "Club actualizado exitosamente" };
        }
    }
}
