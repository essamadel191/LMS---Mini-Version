using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Enrollments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Enrollments.Handlers
{
    public class GetEnrollmentsByInternQueryHandler : IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;

        public GetEnrollmentsByInternQueryHandler(IGeneralRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsByInternQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository.GetAll().Where(enroll => enroll.InternId == request.InternId).Select(
                enrollment => new EnrollmentDto()
                {
                    Id = enrollment.Id,
                    InternId = enrollment.InternId,
                    InternName = enrollment.Intern.FullName,
                    TrackId = enrollment.TrackId,
                    TrackName = enrollment.Track.Name,
                    EnrollmentDate = enrollment.EnrollmentDate,
                    Status = enrollment.Status
            }).ToListAsync(cancellationToken);
        }
    }
}
