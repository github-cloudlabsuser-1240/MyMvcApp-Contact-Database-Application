using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Xunit;

namespace MyMvcApp.Tests
{
    public class UserControllerTest
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfUsers()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Equal(UserController.userlist, model);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsAViewResult_WithAUser()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal(user, model);
        }

        [Fact]
        public void Create_Post_ReturnsARedirectAndAddsUser_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 2, Name = "New User", Email = "new@example.com" };

            // Act
            var result = controller.Create(user);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Contains(user, UserController.userlist);
        }

        [Fact]
        public void Edit_Post_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 3, Name = "Edit User", Email = "edit@example.com" };

            // Act
            var result = controller.Edit(0, user);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsARedirectAndUpdatesUser_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 4, Name = "Existing User", Email = "existing@example.com" };
            UserController.userlist.Add(user);
            var updatedUser = new User { Id = 4, Name = "Updated User", Email = "updated@example.com" };

            // Act
            var result = controller.Edit(4, updatedUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Updated User", user.Name);
            Assert.Equal("updated@example.com", user.Email);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Delete(0, null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_ReturnsARedirectAndRemovesUser_WhenIdIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 5, Name = "Delete User", Email = "delete@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = controller.Delete(5, null);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.DoesNotContain(user, UserController.userlist);
        }
    }
}
