using Application.Shared.Exceptions;
using Event.Application.Command.EventMember.PaticipateMember;
using Event.Domain.Common;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class PaticipateMemberTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IEventRepository> eventRepositoryMock;
        private readonly Mock<IEventMemberRepository> eventMemberRepositoryMock;
        private readonly PaticipateMemberHandler paticipateMemberHandler;

        public PaticipateMemberTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            eventRepositoryMock = new Mock<IEventRepository>();
            eventMemberRepositoryMock = new Mock<IEventMemberRepository>();

            unitOfWorkMock
                .Setup(x => x.EventRepository)
                .Returns(eventRepositoryMock.Object);

            unitOfWorkMock
                .Setup(x => x.EventMemberRepository)
                .Returns(eventMemberRepositoryMock.Object);

            paticipateMemberHandler = new PaticipateMemberHandler(unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_InvalidCommandEventNotExist_ThrowBadRequestException()
        {
            // Arrange
            var request = new PaticipateMemberCommand
            {
                Email = "test",
                EventId = 12,
                FirstName = "tst",
                SecondName = "test2",
                BirthDate = new DateTime(2005, 8, 22)
            };
            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(request.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EventEntity)null);

            // Act and Assert
            var ex = async () => 
            await paticipateMemberHandler
                .Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<BadRequestApiException>()
                .WithMessage("Event Does Not Exist");
        }

        [Fact]
        public async Task Handle_EventIsFull_ThrowConflictException()
        {
            // Arrange
            var request = new PaticipateMemberCommand
            {
                Email = "test",
                EventId = 12,
                FirstName = "tst",
                SecondName = "test2",
                BirthDate = new DateTime(2005, 8, 22)
            };
            var eventMember = EventMember.Initialize(
                "test", "test", "test@mail.ru");
            var eventEntity = EventEntity.Initialize(
                "test", "desc", "loc", "cat", 1, DateTime.Now);
            eventEntity.Value.Members.Add(eventMember.Value);
            
            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(request.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity.Value);

            // Act and Assert
            var ex = async () => await paticipateMemberHandler
                .Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<ConflictApiException>()
                .WithMessage("Event Is Full");
        }

        [Fact]
        public async Task Handle_EventAlreadyRegistered_ThrowConflictException()
        {
            // Arrange
            var request = new PaticipateMemberCommand
            {
                Email = "test@mail.ru",
                EventId = 12,
                FirstName = "tst",
                SecondName = "test2",
                BirthDate = new DateTime(2005, 8, 22)
            };
            var eventMember = EventMember.Initialize(
                "test", "test", "test@mail.ru");
            var eventEntity = EventEntity.Initialize(
                "test", "desc", "loc", "cat", 3, DateTime.Now);
            eventEntity.Value.Members.Add(eventMember.Value);

            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(request.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity.Value);

            // Act and Assert
            var ex = async () => await paticipateMemberHandler
                .Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<ConflictApiException>()
                .WithMessage("Current Email Already Registered On This Event");
        }

        [Fact]
        public async Task Handle_InvalidRequest_ThrowBadRequestException()
        {
            // Arrange
            var request = new PaticipateMemberCommand
            {
                Email = "test@mail",
                EventId = 12,
                FirstName = "tst",
                SecondName = "test2",
                BirthDate = new DateTime(2005, 8, 22)
            };
            var eventEntity = EventEntity.Initialize(
                "test", "desc", "loc", "cat", 3, DateTime.Now);

            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(request.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity.Value);

            // Act and Assert
            var ex = async () => await paticipateMemberHandler
                .Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<BadRequestApiException>();
        }

        [Fact]
        public async Task Handle_AddNewEventMember_ReturnIdMember()
        {
            // Arrange
            var request = new PaticipateMemberCommand
            {
                Email = "test@mail.ru",
                EventId = 12,
                FirstName = "tst",
                SecondName = "test2",
                BirthDate = new DateTime(2005, 8, 22)
            };
            var eventEntity = EventEntity.Initialize(
                "test", "desc", "loc", "cat", 3, DateTime.Now);

            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(request.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity.Value);

            // Act and Assert
            var ex = async () => await paticipateMemberHandler
                .Handle(request, CancellationToken.None);

            // Assert
            await ex.Should().NotThrowAsync();
            unitOfWorkMock.Verify(x => x.SaveChangesAsync());
        }
    }
}
