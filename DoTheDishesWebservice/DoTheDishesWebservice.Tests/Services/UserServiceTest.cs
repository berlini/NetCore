using DoTheDishesWebservice.Core.Services;
using DoTheDishesWebservice.Core.Utils;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DoTheDishesWebservice.Tests.Services
{
    public class UserServiceTest
    {
        #region Get
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Get_IdLesserOne_ReturnsArgumentException(int userId)
        {
            var userRepositoryStub = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryStub.Object, homeRepositoryStub.Object, loggingStub.Object);

            Assert.Throws<ArgumentException>(() => userService.Get(userId));
        }

        [Fact]
        public void Get_IdGreaterZero_ReturnsObject()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();

            userRepositoryMock.Setup(o => o.Get(It.IsAny<int>())).Returns(new User
            {
                UserId = 1
            });

            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);
            User user = userService.Get(1);

            Assert.Equal(1, user.UserId);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAll_ReturnsEmpty()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();

            userRepositoryMock.Setup(o => o.GetAll()).Returns(new List<User>());

            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);
            IEnumerable<User> users = userService.GetAll();

            Assert.Empty(users);
        }

        [Fact]
        public void GetAll_ReturnsThree()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();

            userRepositoryMock.Setup(o => o.GetAll()).Returns(new List<User>
            {
                new User(),
                new User(),
                new User()
            });

            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);
            List<User> users = userService.GetAll() as List<User>;

            Assert.Equal(3, users.Count);
        }
        #endregion

        #region Create (home parameter)
        [Theory]
        [InlineData("", "a", "a", 1)]
        [InlineData(null, "a", "a", 1)]
        [InlineData("a", "", "a", 1)]
        [InlineData("a", null, "a", 1)]
        [InlineData("a", "a", "", 1)]
        [InlineData("a", "a", null, 1)]
        [InlineData("", "", "", 1)]
        [InlineData(null, null, null, 1)]
        public void CreateWithHome_InvalidParameters_ReturnsArgumentException(string login, string password, string nickname, int homeId)
        {
            var userRepositoryStub = new Mock<IUserRepository>();
            var homeRepositoryMock = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryStub.Object, homeRepositoryMock.Object, loggingStub.Object);

            homeRepositoryMock.Setup(o => o.Get(It.IsAny<int>())).Returns(new Home ());

            Assert.Throws<ArgumentException>(() => userService.Create(login, password, nickname, homeId));
        }

        [Fact]
        public void CreateWithHome_HomeDoesntExist_ReturnsApplicationException()
        {
            var userRepositoryStub = new Mock<IUserRepository>();
            var homeRepositoryMock = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryStub.Object, homeRepositoryMock.Object, loggingStub.Object);

            homeRepositoryMock.Setup(o => o.Get(It.IsAny<int>())).Returns((Home)null);

            string login = "a";
            string password = "a";
            string nickname = "a";
            int homeId = 0;

            Assert.Throws<ApplicationException>(() => userService.Create(login, password, nickname, homeId));
        }

        [Fact]
        public void CreateWithHome_ReturnsCreatedUserWithId()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryMock = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryMock.Object, loggingStub.Object);

            homeRepositoryMock.Setup(o => o.Get(It.IsAny<int>()))
                .Returns(new Home { HomeId = 1 });

            userRepositoryMock.Setup(o => o.Save(It.IsAny<User>()))
                .Callback((User u) => { u.UserId = 1; })
                .Verifiable();

            User userExpected = new User
            {
                UserId = 1,
                Login = "a",
                Password = Criptography.GetSHA1("a"),
                Nickname = "a",
                Home = new Home
                {
                    HomeId = 1
                }
            };

            string login = "a";
            string password = "a";
            string nickname = "a";
            int homeId = 1;

            User userSaved = userService.Create(login, password, nickname, homeId);

            string userExpectedString = JsonConvert.SerializeObject(userExpected);
            string userSavedString = JsonConvert.SerializeObject(userSaved);

            Assert.Equal(userExpectedString, userSavedString);
        }
        #endregion

        #region Create (no home parameter)
        [Theory]
        [InlineData("", "a", "a")]
        [InlineData(null, "a", "a")]
        [InlineData("a", "", "a")]
        [InlineData("a", null, "a")]
        [InlineData("a", "a", "")]
        [InlineData("a", "a", null)]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        public void CreateWithNoHome_InvalidParameters_ReturnsArgumentException(string login, string password, string nickname)
        {
            var userRepositoryStub = new Mock<IUserRepository>();
            var homeRepositoryMock = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryStub.Object, homeRepositoryMock.Object, loggingStub.Object);

            Assert.Throws<ArgumentException>(() => userService.Create(login, password, nickname));
        }

        [Fact]
        public void CreateWithNoHome_ReturnsCreatedUserWithId()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryMock = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryMock.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.Save(It.IsAny<User>()))
                .Callback((User u) => { u.UserId = 1; })
                .Verifiable();

            User userExpected = new User
            {
                UserId = 1,
                Login = "a",
                Password = Criptography.GetSHA1("a"),
                Nickname = "a"
            };

            string login = "a";
            string password = "a";
            string nickname = "a";

            User userSaved = userService.Create(login, password, nickname);

            string userExpectedString = JsonConvert.SerializeObject(userExpected);
            string userSavedString = JsonConvert.SerializeObject(userSaved);

            Assert.Equal(userExpectedString, userSavedString);
        }
        #endregion

        #region Delete
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Delete_NoException(int idToDelete)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);
            List<User> listUser = new List<User>
            {
                new User { UserId = 1 },
                new User { UserId = 2 },
                new User { UserId = 3 }
            };

            userRepositoryMock.Setup(o => o.CheckIfExists(It.IsAny<int>())).Returns(listUser.Exists(o => o.UserId == idToDelete));
            userRepositoryMock.Setup(o => o.Delete(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    User u = listUser.Find(o => o.UserId == id);
                    listUser.Remove(u);
                })
                .Verifiable();
            
            userService.Delete(idToDelete);

            Assert.False(listUser.Exists(o => o.UserId == idToDelete));
        }
        #endregion

        #region CheckIfExists
        [Fact]
        public void CheckIfExists_ReturnTrue()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.CheckIfExists(It.IsAny<int>())).Returns(true);

            int idToCheck = 1;
            bool ret = userService.CheckIfExists(idToCheck);

            Assert.True(ret);
        }

        [Fact]
        public void CheckIfExists_ReturnFalse()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.CheckIfExists(It.IsAny<int>())).Returns(false);

            int idToCheck = 1;
            bool ret = userService.CheckIfExists(idToCheck);

            Assert.False(ret);
        }
        #endregion

        #region Login
        [Theory]
        [InlineData("", "a")]
        [InlineData(null, "a")]
        [InlineData("a", "")]
        [InlineData("a", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Login_InvalidParameters_ReturnsArgumentException(string login, string password)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.GetUserByLogin(It.IsAny<string>())).Returns(new User());

            Assert.Throws<ArgumentException>(() => userService.Login(login, password));
        }

        [Fact]
        public void Login_UserNotFound_ReturnsApplicationException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.GetUserByLogin(It.IsAny<string>())).Returns((User) null);

            string login = "a";
            string password = "a";

            Assert.Throws<ApplicationException>(() => userService.Login(login, password));
        }

        [Fact]
        public void Login_InvalidLogin_ReturnsApplicationException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.GetUserByLogin(It.IsAny<string>())).Returns(new User
            {
                Login = "b",
                Password = Criptography.GetSHA1("a")
            });

            string login = "a";
            string password = "a";

            Assert.Throws<ApplicationException>(() => userService.Login(login, password));
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsApplicationException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.GetUserByLogin(It.IsAny<string>())).Returns(new User
            {
                Login = "a",
                Password = Criptography.GetSHA1("b")
            });

            string login = "a";
            string password = "a";

            Assert.Throws<ApplicationException>(() => userService.Login(login, password));
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsUser()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var homeRepositoryStub = new Mock<IHomeRepository>();
            var loggingStub = new Mock<ILogger<UsersService>>();
            UsersService userService = new UsersService(userRepositoryMock.Object, homeRepositoryStub.Object, loggingStub.Object);

            userRepositoryMock.Setup(o => o.GetUserByLogin(It.IsAny<string>())).Returns(new User
            {
                UserId = 1,
                Login = "a",
                Password = Criptography.GetSHA1("a")
            });

            string login = "a";
            string password = "a";
            User user = userService.Login(login, password);

            Assert.Equal(1, user.UserId);
        }
        #endregion
    }
}
