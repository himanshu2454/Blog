using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Blog.Core.Repository;
using Blog.Core.Services;
using Blog.Domain.Entities;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Blog.Performance.Tests
{
    [SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput, launchCount: 1, warmupCount: 1, iterationCount: 3)]
    [MemoryDiagnoser]
    public class InliningBenchmark
    {
        private IPostService _postService;
        private Mock<IPostRepo> _postRepoMock;
        private Mock<ILogger<PostService>> _loggerMock;
        private string _testId;

        [GlobalSetup]
        public void Setup()
        {
            _postRepoMock = new Mock<IPostRepo>();
            _loggerMock = new Mock<ILogger<PostService>>();
            _testId = "test-id";
            _postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Expression<Func<Post, bool>>>()))
                .ReturnsAsync(new Post { Id = _testId });
            _postService = new PostService(_postRepoMock.Object, _loggerMock.Object);
        }

        [Benchmark]
        public async Task<Result<Post>> Benchmark_GetByIdAsync()
        {
            return await _postService.GetByIdAsync(_testId);
        }

        [Benchmark]
        public async Task<Result<Post>> Benchmark_GetByIdAsyncAggressiveInlining()
        {
            return await _postService.GetByIdAsyncAggressiveInlining(_testId);
        }

    }
}