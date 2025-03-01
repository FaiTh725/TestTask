using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Models.Members;
using Event.Application.Queries.EventMember.GetMembersPagination;
using Event.Domain.Common;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using EventServices.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class GetMembersPaginationTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IEventRepository> eventRepositoryMock;
        private readonly Mock<IEventMemberRepository> memberRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly GetMembersPaginationHandler handler;

        public GetMembersPaginationTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            eventRepositoryMock = new Mock<IEventRepository>();
            memberRepositoryMock = new Mock<IEventMemberRepository>();
            mapperMock = new Mock<IMapper>();

            unitOfWorkMock
                .Setup(x => x.EventRepository)
                .Returns(eventRepositoryMock.Object);
            unitOfWorkMock
                .Setup(x => x.EventMemberRepository)
                .Returns(memberRepositoryMock.Object);

            handler = new GetMembersPaginationHandler(
                unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EventNotExist_ThrowNotFoundException()
        {
            // Arrange
            var request = new GetMembersPaginationQuery 
            { 
                EventId = 1,
                Page = 1,
                Size = 2
            };
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
        public async Task Handle_GetMembersPagination_EventMembers()
        {
            // Arrange
            var request = new GetMembersPaginationQuery
            {
                EventId = 1,
                Page = 1,
                Size = 2
            };
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

            memberRepositoryMock
                .Setup(x => x.GetEventMembers(
                    request.EventId,
                    request.Page,
                    request.Size))
                .Returns([member1, member2]);

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
            memberRepositoryMock.Verify(x => x.GetEventMembers(
                request.EventId,
                request.Page,
                request.Size));
            mapperMock.Verify(x => x.Map<IEnumerable<MemberResponse>>(eventEntity.Members));
        }
    }
}
