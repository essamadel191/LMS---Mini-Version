using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepository
            , IUnitOfWork unitOfWork
            )
        {
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetTable().Where(track => track.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (track == null) throw new Exception($"Track Not Found Id {request.Id}");

            // there is strict delete so I must check to prevent exception
            //if (track.Enrollments.Any()) return;

            _trackRepository.Delete(track);
            var result = await _unitOfWork.CompleteAsync();
            // how will we save changes ??

            return;
        }
    }
}
