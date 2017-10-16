using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services;

namespace TelerikMovies.Tests.TelerikMoviesServices.VoteServiceTests
{
    [TestClass]
    public class LikeOrDislikeAMovie
    {
        private Mock<IEfGenericRepository<Movies>> movies;
        private Mock<IEfGenericRepository<Genres>> genresRepo;
        private Mock<IEfGenericRepository<Comments>> commentsRepo;
        private Mock<IEfGenericRepository<Users>> userRepo;
        private Mock<IEfGenericRepository<Likes>> likesRepo;
        private Mock<IEfGenericRepository<Dislikes>> dislikesRepo;
        private Mock<IUoW> saver;
        [TestInitialize]
        public void Startup()
        {
            movies = new Mock<IEfGenericRepository<Movies>>();
            genresRepo = new Mock<IEfGenericRepository<Genres>>();
            commentsRepo = new Mock<IEfGenericRepository<Comments>>();
            userRepo = new Mock<IEfGenericRepository<Users>>();
            likesRepo = new Mock<IEfGenericRepository<Likes>>();
            dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
            saver = new Mock<IUoW>();
        }

        [TestMethod]
        public void IfIsAlreadyLikeReturnErrorResult()
        {
            //Arrange
            List<Likes> likes = new List<Likes>();
            var like1 = new Likes
            {
                User = new Users()
                {
                    UserName = "User1"
                },
                Movie = new Movies()
                {
                    Id = Guid.NewGuid()
                }
            };
            var like2 = new Likes
            {
                User = new Users()
                {
                    UserName = "User2"
                },
                Movie = new Movies()
                {
                    Id = Guid.NewGuid()
                }
            };
            likes.Add(like1);
            likes.Add(like2);
            likesRepo.Setup(x => x.All()).Returns(likes.AsQueryable());
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.LikeOrDislikeAMovie(like1.User.UserName,like1.Movie.Id,true);

            //Assert
            likesRepo.Verify(x => x.All(), Times.Once);
            Assert.AreEqual(result.ResulType, ResultType.AlreadyExists);
            Assert.AreEqual(result.ErrorMsg, Constants.ThisUserAlreadyLikedOrDisliked);

        }

        [TestMethod]
        public void IfIsAlreadyDislikedReturnErrorResult()
        {
            //Arrange
            List<Dislikes> dislikes = new List<Dislikes>();
            var dislike1 = new Dislikes
            {
                User = new Users()
                {
                    UserName = "User1"
                },
                Movie = new Movies()
                {
                    Id = Guid.NewGuid()
                }
            };
            var dislike2 = new Dislikes
            {
                User = new Users()
                {
                    UserName = "User2"
                },
                Movie = new Movies()
                {
                    Id = Guid.NewGuid()
                }
            };
            dislikes.Add(dislike1);
            dislikes.Add(dislike2);
            dislikesRepo.Setup(x => x.All()).Returns(dislikes.AsQueryable());
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.LikeOrDislikeAMovie(dislike1.User.UserName, dislike1.Movie.Id, false);

            //Assert
            likesRepo.Verify(x => x.All(), Times.Exactly(1));
            Assert.AreEqual(result.ResulType, ResultType.AlreadyExists);
            Assert.AreEqual(result.ErrorMsg, Constants.ThisUserAlreadyLikedOrDisliked);

        }
        [TestMethod]
        public void IfItIsNotAlreadyLikedOrDislikedReturnResultChangedIfUserNotFound()
        {
            //Arrange
            List<Likes> likes = new List<Likes>();  
            likesRepo.Setup(x => x.All()).Returns(likes.AsQueryable());    
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn =(Users)null;
            //Act
            var result = sut.LikeOrDislikeAMovie("User1", Guid.NewGuid(), true);

            //Assert
            Assert.AreEqual(sut.GetCurrentUserCalled, 1);
            Assert.AreEqual(result.ErrorMsg,"Changed");

        }

        [TestMethod]
        public void IfItIsNotAlreadyLikedOrDislikedReturnResultChangedIfMovieNotFound()
        {
            //Arrange
            List<Likes> likes = new List<Likes>();
            likesRepo.Setup(x => x.All()).Returns(likes.AsQueryable());
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = new Users();
            sut.MovieToReturn = (Movies)null;
            //Act
            var result = sut.LikeOrDislikeAMovie("User1", Guid.NewGuid(), true);

            //Assert
            Assert.AreEqual(sut.GetCurrentUserCalled, 1);
            Assert.AreEqual(sut.GetMovieCount, 1);
            Assert.AreEqual(result.ErrorMsg, "Changed");

        }
        [TestMethod]
        public void IfItIsNotAlreadyLikedOrDislikedMovieExistsAndUsersExistsAndItIsLikeCallLikesRepoAdd()
        {
            //Arrange
            var called = 0;
            var guid = Guid.NewGuid();
            List<Likes> likes = new List<Likes>();
            likesRepo.Setup(x => x.All()).Returns(likes.AsQueryable());
            likesRepo.Setup(x => x.Add(It.IsAny<Likes>())).Callback(() => { called++; });
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = new Users()
            {
                UserName="UserName1"
            };
            sut.MovieToReturn = new Movies()
            {
                Id = guid
            };
            //Act
            var result = sut.LikeOrDislikeAMovie("User1", guid, true);

            //Assert
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.GetCurrentUserCalled, 1);
            Assert.AreEqual(sut.GetMovieCount, 1);
            Assert.AreEqual(sut.SaveChangesCalled, 1);
            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(result.ResulType, ResultType.Success);

        }

        [TestMethod]
        public void IfItIsNotAlreadyLikedOrDislikedMovieExistsAndUsersExistsAndItIsDislikeCallDislikesRepoAdd()
        {
            //Arrange
            var called = 0;
            var guid = Guid.NewGuid();
            List<Dislikes> dislikes = new List<Dislikes>();
            dislikesRepo.Setup(x => x.All()).Returns(dislikes.AsQueryable());
            dislikesRepo.Setup(x => x.Add(It.IsAny<Dislikes>())).Callback(() => { called++; });
            var sut = new VoteServiceTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = new Users()
            {
                UserName = "UserName1"
            };
            sut.MovieToReturn = new Movies()
            {
                Id = guid
            };
            //Act
            var result = sut.LikeOrDislikeAMovie("User1", guid, false);

            //Assert
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.GetCurrentUserCalled, 1);
            Assert.AreEqual(sut.GetMovieCount, 1);
            Assert.AreEqual(sut.SaveChangesCalled, 1);
            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(result.ResulType, ResultType.Success);

        }
        private class VoteServiceTest : VoteService
        {
            public VoteServiceTest(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
           IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
           IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
            : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
            {
                this.GetCurrentUserCalled = 0;
                this.SaveChangesCalled = 0;
                this.GetMovieCount = 0;
            }

            public int GetCurrentUserCalled { get; set; }

            public int GetMovieCount { get; set; }
            public int SaveChangesCalled { get; set; }

            public Action SaveChangesActionCalled { get; set; }

            public Users UserToReturn { get; set; }
            public Movies MovieToReturn { get; set; }
            public override Users GetCurrentUser(string userName, ref IResult result)
            {
                this.GetCurrentUserCalled++;
                result.ErrorMsg = "Changed";
                return UserToReturn;
            }
            public override Movies GetMovie(Guid movieId, ref IResult result)
            {
                this.GetMovieCount++;
                result.ErrorMsg = "Changed";
                return MovieToReturn;
            }
            public override void SaveChange(Action action, ref IResult result)
            {
                action();
                result.ErrorMsg = "Changed";
                this.SaveChangesCalled++;
            }
        }
    }
}
