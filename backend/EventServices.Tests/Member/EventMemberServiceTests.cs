using Application.Shared.Enums;
using Application.Shared.Responses;
using AutoMapper.Execution;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Entities;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class EventMemberServiceTests
    {
        private readonly Mock<IMemberService> memberServiceMock;

        public EventMemberServiceTests()
        {
            memberServiceMock = new Mock<IMemberService>();
        }

        [Fact]
        public async Task GetMemberEvent_EventNotFound_ReturnNotFoundResponse()
        {
            // Arrange
            long eventId = 1;
            var expectedResponse = new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.NotFound,
                Description = "",
                Data = new List<MemberResponse>()
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent(eventId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Data.Should().BeEmpty();
            result.Description.Should().NotBeNull();
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId));
        }

        [Fact]
        public async Task GetMemberEvent_ExistData_ReturnOkResponse()
        {
            // Arrange
            var expectedEventMembers = new List<MemberResponse>
            {
                new MemberResponse { Id = 1, Email = "Test@mail.ru", FirstName = "Petr", SecondName = "Ivanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
                new MemberResponse { Id = 3, Email = "NePetr@mail.ru", FirstName = "NePetr", SecondName = "NeIvanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
            };

            long eventId = 1;
            var expectedResponse = new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.Ok,
                Description = "",
                Data = expectedEventMembers
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent(eventId);
        
            // Assert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(expectedEventMembers);
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId));
        }

        [Fact]
        public async Task GetEventMember_NotFound_ReturnNotFoundResponse()
        {
            // Arrange
            var memberId = 1;
            var expectedResponse = new DataResponse<MemberResponse>
            {
                Data = new(),
                StatusCode = StatusCode.NotFound,
                Description = ""
            };

            memberServiceMock
                .Setup(x => x.GetMember(memberId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .GetMember(memberId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(new MemberResponse());
            memberServiceMock.Verify(x => x.GetMember(memberId));
        }

        [Fact]
        public async Task GetEventMember_ExistedEventMember_ReturnOkResponse()
        {
            // Arrange
            var existedMember = new MemberResponse
            {
                Id = 1,
                Email = "Test@mail.ru",
                FirstName = "Petr",
                SecondName = "Ivanov",
                RegistrationDate = DateTime.UtcNow,
                BirthDate = new DateTime(2005, 08, 22)
            };
            var memberId = 1;
            var expectedResponse = new DataResponse<MemberResponse>
            {
                Data = existedMember,
                StatusCode = StatusCode.Ok,
                Description = ""
            };

            memberServiceMock
                .Setup(x => x.GetMember(memberId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .GetMember(memberId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(existedMember);
            memberServiceMock.Verify(x => x.GetMember(memberId));
        }

        [Fact]
        public async Task CancelMemberPaticipation_MemberNotFound_ReturnNotFoundException()
        {
            // Arrange
            long memberId = 1;
            var expectedResponse = new BaseResponse
            {
                StatusCode = StatusCode.NotFound,
                Description = ""
            };

            memberServiceMock
                .Setup(x => x.CancelMemberParticipation(memberId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .CancelMemberParticipation(memberId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Description.Should().NotBeNull();
            memberServiceMock.Verify(x => x.CancelMemberParticipation(memberId));
        }

        [Fact]
        public async Task CancelMemberPaticipation_MemberExist_ReturnOkResponse()
        {
            // Arrange
            long memberId = 1;
            var expectedResponse = new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = ""
            };

            memberServiceMock
                .Setup(x => x.CancelMemberParticipation(memberId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .CancelMemberParticipation(memberId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            memberServiceMock.Verify(x => x.CancelMemberParticipation(memberId));
        }

        [Fact]
        public async Task GetMembersEvent_EventNotFound_ReturnNotFoundException()
        {
            // Arrange
            long eventId = 1;
            var expectedResult = new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.NotFound,
                Description = "",
                Data = new List<MemberResponse>()
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent(eventId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Description.Should().NotBeNull();
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId));
        }

        [Fact]
        public async Task GetMembersEvent_MembersOfEvent_ReturnOkResponse()
        {
            // Arrange
            var evntMembers = new List<MemberResponse>()
            {
                new MemberResponse { Id = 1, Email = "Test@mail.ru", FirstName = "Petr", SecondName = "Ivanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
                new MemberResponse { Id = 3, Email = "NePetr@mail.ru", FirstName = "NePetr", SecondName = "NeIvanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
            };
            var eventId = 1;
            var expectedResult = new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.Ok,
                Description = "",
                Data = evntMembers
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent(eventId);

            // Assert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(evntMembers);
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId));
        }

        [Fact]
        public async Task AddEventMember_EventNotFound_ReturnNotFoundException()
        {
            // Arrange
            var eventId = 1;
            var request = new MemberRequest
            {
                FirstName = "TestName",
                SecondName = "TestSecondName",
                Email = "test@email.ru",
                BirthDate = new DateTime(2005, 08, 22)
            };

            var expectedResponse = new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.NotFound,
                Description = "",
                Data = new()
            };

            memberServiceMock
                .Setup(x => x.AddEventMember(eventId, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .AddEventMember(eventId, request);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(new MemberResponse());
            memberServiceMock.Verify(x => x.AddEventMember(eventId, request));
        }

        [Fact]
        public async Task AddEventMember_EventIsFull_ReturnBadRequestException()
        {
            // Arrange
            long eventId = 1;
            var request = new MemberRequest
            {
                FirstName = "TestName",
                SecondName = "TestSecondName",
                Email = "test@email.ru",
                BirthDate = new DateTime(2005, 08, 22)
            };

            var expectedResponse = new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.BadRequest,
                Description = "Event Is Full",
                Data = new()
            };

            memberServiceMock
                .Setup(x => x.AddEventMember(eventId, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .AddEventMember(eventId, request);

            // Assert
            result.StatusCode.Should().Be(StatusCode.BadRequest);
            result.Description.Should().Be("Event Is Full");
            result.Data.Should().BeEquivalentTo(new MemberResponse());
            memberServiceMock.Verify(x => x.AddEventMember(eventId, request));
        }

        [Fact]
        public async Task AddEventMember_EmailAlreadyRegistered_ReturnBadRequestException()
        {
            // Arrange
            var eventId = 1;
            var request = new MemberRequest
            {
                FirstName = "TestName",
                SecondName = "TestSecondName",
                Email = "test@email.ru",
                BirthDate = new DateTime(2005, 08, 22)
            };

            var expectedResponse = new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.BadRequest,
                Description = "Current Email Already Registered On This Event",
                Data = new()
            };

            memberServiceMock
                .Setup(x => x.AddEventMember(eventId, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .AddEventMember(eventId, request);

            // Assert
            result.StatusCode.Should().Be(StatusCode.BadRequest);
            result.Description.Should().Be("Current Email Already Registered On This Event");
            result.Data.Should().BeEquivalentTo(new MemberResponse());
            memberServiceMock.Verify(x => x.AddEventMember(eventId, request));
        }

        [Fact]
        public async Task AddEventmember_SuccessAdd_ReturnOkResponse()
        {
            // Arrange
            var eventId = 1;
            var request = new MemberRequest
            {
                FirstName = "TestName",
                SecondName = "TestSecondName",
                Email = "test@email.ru",
                BirthDate = new DateTime(2005, 08, 22)
            };

            var memberResponse = new MemberResponse
            {
                Id = 2,
                BirthDate = new DateTime(2005, 08, 22),
                FirstName = "TestName",
                SecondName = "TestSecondName",
                Email = "test@email.ru",
                RegistrationDate = DateTime.UtcNow
            };

            var expectedResponse = new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "",
                Data = memberResponse
            };

            memberServiceMock
                .Setup(x => x.AddEventMember(eventId, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .AddEventMember(eventId, request);

            // Assert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(memberResponse);
            memberServiceMock.Verify(x => x.AddEventMember(eventId, request));
        }

        [Fact]
        public async Task AddEventmember_RequestIsInvalid_ReturnBadRequestException()
        {
            // Arrange
            var eventId = 1;
            var request = new MemberRequest
            {
                FirstName = "TestName",
                SecondName = "",
                Email = "test.ru",
                BirthDate = new DateTime(2005, 08, 22)
            };

            var expectedResponse = new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.BadRequest,
                Description = "",
                Data = new ()
            };

            memberServiceMock
                .Setup(x => x.AddEventMember(eventId, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .AddEventMember(eventId, request);

            // Assert
            result.StatusCode.Should().Be(StatusCode.BadRequest);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(new MemberResponse());
            memberServiceMock.Verify(x => x.AddEventMember(eventId, request));
        }

        [Fact]
        public async Task GetMembersEventPagination_ExistedEventMemberts_ReturnOkResponse()
        {
            // Arrange
            var eventId = 1;
            var evntMembers = new List<MemberResponse>()
            {
                new MemberResponse { Id = 1, Email = "Test@mail.ru", FirstName = "Petr", SecondName = "Ivanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
                new MemberResponse { Id = 3, Email = "NePetr@mail.ru", FirstName = "NePetr", SecondName = "NeIvanov", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
                new MemberResponse { Id = 5, Email = "test2@mail.ru", FirstName = "fgfg", SecondName = "fsdf", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
                new MemberResponse { Id = 7, Email = "test3@mail.ru", FirstName = "fsdsadf", SecondName = "fdsfsdf", RegistrationDate = DateTime.UtcNow, BirthDate = new DateTime(2005, 08, 22)},
            };
            var page = 1;
            var size = 4;

            var expectedResponse = new DataResponse<IEnumerable<MemberResponse>>
            { 
                StatusCode = StatusCode.Ok,
                Description = "",
                Data = evntMembers
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId, page, size))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent (eventId, page, size);

            // Accert
            result.StatusCode.Should().Be(StatusCode.Ok);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(evntMembers);
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId, page, size));
        }

        [Fact]
        public async Task GetMembersEventPagination_EventNotFound_ReturnNotFoundException()
        {
            // Arrange
            var eventId = 1;
            var page = 1;
            var size = 4;

            var expectedResponse = new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.NotFound,
                Description = "",
                Data = new List<MemberResponse>()
            };

            memberServiceMock
                .Setup(x => x.GetMembersEvent(eventId, page, size))
                .ReturnsAsync(expectedResponse);


            // Act
            var result = await memberServiceMock.Object
                .GetMembersEvent(eventId, page, size);

            // Assert
            result.StatusCode.Should().Be(StatusCode.NotFound);
            result.Description.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            memberServiceMock.Verify(x => x.GetMembersEvent(eventId, page, size));
        }
    }
}
