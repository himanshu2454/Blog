using Blog.Core.Repository;
using Blog.Core.Services;
using Blog.Domain.Entities;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Unit.Tests
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepo> _postRepoMock;
        private readonly Mock<ILogger<PostService>> _loggerMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepoMock = new Mock<IPostRepo>();
            _loggerMock = new Mock<ILogger<PostService>>();
            _postService = new PostService(_postRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenPostIsNull()
        {
            var result = await _postService.CreateAsync(null);
            Assert.True(result.IsFailure);
            Assert.Equal("Post cannot be null.", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnSuccess_WhenPostIsValid()
        {
            var post = new Post { Id = Guid.NewGuid().ToString() };
            _postRepoMock.Setup(r => r.CreateAsync(post)).Returns(Task.CompletedTask);

            var result = await _postService.CreateAsync(post);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFailure_WhenPostNotFound()
        {
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync((Post)null);

            var result = await _postService.GetByIdAsync("notfound");

            Assert.True(result.IsFailure);
            Assert.Equal("Post not found.", result.Error);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess_WhenPostFound()
        {
            var post = new Post { Id = "found" };
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync(post);

            var result = await _postService.GetByIdAsync("found");

            Assert.True(result.IsSuccess);
            Assert.Equal(post, result.Value);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPosts()
        {
            var posts = new List<Post> { new Post { Id = "1" }, new Post { Id = "2" } };
            _postRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(posts);

            var result = await _postService.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(posts, result.Value);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenPostIsNull()
        {
            var result = await _postService.UpdateAsync(null);

            Assert.True(result.IsFailure);
            Assert.Equal("Post cannot be null.", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFailure_WhenPostNotFound()
        {
            var post = new Post { Id = "notfound" };
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync((Post)null);

            var result = await _postService.UpdateAsync(post);

            Assert.True(result.IsFailure);
            Assert.Equal("Post not found.", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenPostFound()
        {
            var post = new Post { Id = "found" };
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync(post);
            _postRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Expression<Func<Post, bool>>>(), post)).Returns(Task.CompletedTask);

            var result = await _postService.UpdateAsync(post);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenPostNotFound()
        {
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync((Post)null);

            var result = await _postService.DeleteAsync(Guid.NewGuid());

            Assert.True(result.IsFailure);
            Assert.Equal("Post not found.", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenPostFound()
        {
            var post = new Post { Id = Guid.NewGuid().ToString() };
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync(post);
            _postRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Expression<Func<Post, bool>>>())).Returns(Task.CompletedTask);

            var result = await _postService.DeleteAsync(Guid.Parse(post.Id));

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GetByIdAsyncAggressiveInlining_ShouldReturnFailure_WhenPostNotFound()
        {
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync((Post)null);

            var result = await _postService.GetByIdAsyncAggressiveInlining("notfound");

            Assert.True(result.IsFailure);
            Assert.Equal("Post not found.", result.Error);
        }

        [Fact]
        public async Task GetByIdAsyncAggressiveInlining_ShouldReturnSuccess_WhenPostFound()
        {
            var post = new Post { Id = "found" };
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>())).ReturnsAsync(post);

            var result = await _postService.GetByIdAsyncAggressiveInlining("found");

            Assert.True(result.IsSuccess);
            Assert.Equal(post, result.Value);
        }

        [Fact]
        public void TestInlining()
        {
            var result = Add(1, 2); // Must be used so JIT compiles it
            Assert.Equal(3, result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Add(int a, int b) => a + b;
    }
}
