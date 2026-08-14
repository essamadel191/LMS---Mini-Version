using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetAllInternsQueryHandler : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _InternRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _InternRepository = internRepository;
        }
        public async Task<IEnumerable<InternDto>> Handle(GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            #region Old Approach

            //var interns = await _InternRepository.GetAllAsync();
            //if(interns == null)
            //{
            //    return Enumerable.Empty<InternDto>();
            //}

            //// WE SHOULD USE AUTOMAPPER
            //List<InternDto> internList = new List<InternDto>();
            //foreach(var intern in interns)
            //{
            //    var internDto = new InternDto()
            //    {
            //        Id = intern.Id,
            //        FullName = intern.FullName,
            //        Email = intern.Email,
            //        BirthYear = intern.BirthYear,
            //        Status = intern.Status,
            //        TrackId = intern.TrackId,
            //        TrackName = intern.Track.Name
            //    };
            //    internList.Add(internDto);
            //} 
            #endregion

            return await _InternRepository.GetTable().Include(i => i.Track).Select(
                    intern => new InternDto {
                        Id = intern.Id,
                        FullName = intern.FullName,
                        Email = intern.Email,
                        BirthYear = intern.BirthYear,
                        Status = intern.Status,
                        TrackId = intern.TrackId,
                        TrackName = intern.Track.Name
                    }
                ).ToListAsync() ?? [];
        }
    }
}
