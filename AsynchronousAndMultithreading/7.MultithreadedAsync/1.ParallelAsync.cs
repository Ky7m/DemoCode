using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Bogus;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace MultithreadedAsync
{
    public class ParallelAsync : BaseTestHelpersClass
    {
        private readonly ITestOutputHelper _output;
        private readonly Faker _faker;

        private const int AmountOfWords = 100;
        private const int SuggestionsPerWord = 10;

        public ParallelAsync(ITestOutputHelper output) : base(output)
        {
            _output = output;
            _faker = new Faker();
        }

        [Fact]
        public async Task NotOptimalWay()
        {
            var sw = Stopwatch.StartNew();

            var allSuggestions = new List<string>();
            
            var words = await GetWords();
            foreach (var word in words)
            {
                var suggestions = await GetSearchSuggestions(word);
                allSuggestions.AddRange(suggestions);
            }
            
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            Assert.Equal(AmountOfWords * SuggestionsPerWord, allSuggestions.Count);
        }
        
        [Fact]
        public async Task OtherPossibleWay()
        {
            var sw = Stopwatch.StartNew();

            var allSuggestions = new List<string>();
            var tasks = new List<Task<string[]>>();
            
            var words = await GetWords();
            foreach (var word in words)
            {
                var task = GetSearchSuggestions(word);
                tasks.Add(task);
            }

            var aggregatedResult = await Task.WhenAll(tasks);
            
            foreach (var suggestions in aggregatedResult)
            {
                allSuggestions.AddRange(suggestions);
            }
            
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            Assert.Equal(AmountOfWords * SuggestionsPerWord, allSuggestions.Count);
        }
        
        private async Task<string[]> GetWords()
        {
            await Task.Delay(50);
            return _faker.Random.WordsArray(AmountOfWords);
        }
        
        private async Task<string[]> GetSearchSuggestions(string query)
        {
            await Task.Delay(100);
            return _faker.Random.WordsArray(SuggestionsPerWord);
        }
    }
}