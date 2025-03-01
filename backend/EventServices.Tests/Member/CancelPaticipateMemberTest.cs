using Application.Shared.Exceptions;
using Event.Application.Command.EventMember.CancelPaticipateMember;
using Event.Application.Interfaces;
using Event.Domain.Common;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using EventServices.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class CancelPaticipateMemberTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IEventMemberRepository> eventMemberRepositoryMock;
        private readonly Mock<ICachService> cachServiceMock;
        private readonly CancelPaticipateMemberHandler handler;

        public CancelPaticipateMemberTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            eventMemberRepositoryMock = new Mock<IEventMemberRepository>();
            cachServiceMock = new Mock<ICachService>();

            unitOfWorkMock
                .Setup(x => x.EventMemberRepository)
                .Returns(eventMemberRepositoryMock.Object);

            handler = new CancelPaticipateMemberHandler(
                unitOfWorkMock.Object, 
                cachServiceMock.Object);
        }

        [Fact]
        public async Task Handle_MemberNotExist_ThrowNotFoundException()
        {
            // Arrange
            var request = new CancelPaticipateMemberCommand
            {
                MemberId = 12
            };
            eventMemberRepositoryMock
                .Setup(x => x.GetEventMember(
                    request.MemberId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((EventMember)null);

            // Act and Assert
            var ex = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<NotFoundApiException>()
                .WithMessage("Member Does Not Exist");
        }

        [Fact]
        public async Task Handle_RemoveMember_Nothing()
        {
            // Arrange
            var request = new CancelPaticipateMemberCommand
            {
                MemberId = 1
            };
            var memberMock = TestInitializer.GetTestEventMember(
                1,
                "test",
                "test",
                "test@mail.ru",
                DateTime.UtcNow.AddYears(-25));

            eventMemberRepositoryMock
                .Setup(x => x.GetEventMember(
                    request.MemberId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(memberMock);
            eventMemberRepositoryMock
                .Setup(x => x.RemoveEventMember(
                    request.MemberId, 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            cachServiceMock
                .Setup(x => x.RemoveData("Members:" + memberMock.Id))
                .Returns(Task.CompletedTask);


            // Act and Assert
            var ex = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await ex.Should().NotThrowAsync();
            eventMemberRepositoryMock
                .Verify(x => x.RemoveEventMember(
                    request.MemberId, 
                    It.IsAny<CancellationToken>()));
            cachServiceMock
                .Verify(x => x.RemoveData(
                    "Members:" + memberMock.Id));
        }
    }
}
