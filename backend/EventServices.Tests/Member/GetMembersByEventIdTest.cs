using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Models.Members;
using Event.Application.Queries.EventMember.GetMembersByEventId;
using Event.Domain.Common;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using EventServices.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class GetMembersByEventIdTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IEventRepository> eventRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly GetMembersByEventIdHandler handler;

        public GetMembersByEventIdTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            eventRepositoryMock = new Mock<IEventRepository>();
            mapperMock = new Mock<IMapper>();

            unitOfWorkMock
                .Setup(x => x.EventRepository)
                .Returns(eventRepositoryMock.Object);
            handler = new GetMembersByEventIdHandler(
                unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EventNotExist_ThrowNotFoundException()
        {
            // Arrange
            var request = new GetMembersByEventIdQuery{EventId = 1};
            eventRepositoryMock.Setup(x =>
                x.GetEvent(
                    request.EventId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((EventEntity)null);

            // Act and Assert
            var ex = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<NotFoundApiException>()
                .WithMessage("Such Event Does Not Exist");
        }


        [Fact]
        public async Task Handle_GetEventMembers_EventMembers()
        {
            // Arrange
            var request = new GetMembersByEventIdQuery{EventId = 1};
            var member1 = TestInitializer.GetTestEventMember(
                1,
                "test",
                "test",
                "test@mail.ru",
                DateTime.UtcNow.AddYears(-20));
            var member2 = TestInitializer.GetTestEventMember(
                2,
                "test2",
                "test2",
                "test2@mail.ru",
                DateTime.UtcNow.AddYears(-20));
            var eventEntity = TestInitializer.GetTestEvent(
                1,
                "name",
                "desc",
                "loc",
                "cat",
                5,
                DateTime.Now.AddDays(10),
                [member1, member2]);

            var expectdResult = new List<MemberResponse>()
            {
                new MemberResponse {
                    Id = 1,
                    FirstName = "test",
                    SecondName = "test",
                    Email = "test@mail.ru",
                    BirthDate = DateTime.UtcNow.AddYears(-20),
                    RegistrationDate = DateTime.UtcNow
                },
                new MemberResponse {
                    Id = 2,
                    FirstName = "test2",
                    SecondName = "test2",
                    Email = "test2@mail.ru",
                    BirthDate = DateTime.UtcNow.AddYears(-20),
                    RegistrationDate = DateTime.UtcNow
                }
            };

            eventRepositoryMock
                .Setup(x => x.GetEventWithMembers(
                    request.EventId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity);

            mapperMock
                .Setup(x => x.Map<IEnumerable<MemberResponse>>(eventEntity.Members))
                .Returns(expectdResult);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectdResult);
            eventRepositoryMock.Verify(x => x.GetEventWithMembers(
                request.EventId,
                It.IsAny<CancellationToken>()));
            mapperMock.Verify(x => x.Map<IEnumerable<MemberResponse>>(eventEntity.Members));
        }
    }
}
