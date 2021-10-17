using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class ReplaceOptimizationRunner
    {

        [Benchmark]
        public string ReplaceWithoutContains()
        {
            var text = "<span>Clear  Replace can <span>Dog  be optimized. <span>Cat <span>Draw ";
            text = text.Replace("<span>Cat ", "<span>Cats ");
            text = text.Replace("<span>Clear ", "<span>Clears ");
            text = text.Replace("<span>Dog ", "<span>Dogs ");
            text = text.Replace("<span>Draw ", "<span>Draws ");
            return text;
        }

        [Benchmark]
        public string ReplaceWithContains()
        {
            var text = "<span>Clear  Replace can <span>Dog  be optimized. <span>Cat <span>Draw ";
            if (text.Contains("<span>C"))
            {
                text = text.Replace("<span>Cat ", "<span>Cats ");
                text = text.Replace("<span>Clear ", "<span>Clears ");
            }
            if (text.Contains("<span>D"))
            {
                text = text.Replace("<span>Dog ", "<span>Dogs ");
                text = text.Replace("<span>Draw ", "<span>Draws ");
            }
            return text;
        }

        [Benchmark]
        public string ReplaceUsingRegex()
        {
            var text = "<span>Clear  Replace can <span>Dog  be optimized. <span>Cat <span>Draw ";
            text = Regex.Replace(text, "<span>Cat ", "<span>Cats ");
            text = Regex.Replace(text, "<span>Clear ", "<span>Clears ");
            text = Regex.Replace(text, "<span>Dog ", "<span>Dogs ");
            text = Regex.Replace(text, "<span>Draw ", "<span>Draws ");
            return text;
        }

        [Benchmark]
        public string ReplaceUsingCompiledRegex()
        {
            var text = "<span>Clear  Replace can <span>Dog  be optimized. <span>Cat <span>Draw ";
            text = Regex.Replace(text, "<span>Cat ", "<span>Cats ", RegexOptions.Compiled);
            text = Regex.Replace(text, "<span>Clear ", "<span>Clears ", RegexOptions.Compiled);
            text = Regex.Replace(text, "<span>Dog ", "<span>Dogs ", RegexOptions.Compiled);
            text = Regex.Replace(text, "<span>Draw ", "<span>Draws ", RegexOptions.Compiled);
            return text;
        }

        [Benchmark]
        public string ReplaceUsingStringBuilder()
        {
            var text = "<span>Clear  Replace can <span>Dog  be optimized. <span>Cat <span>Draw ";
            var sb = new StringBuilder(text, text.Length);
            sb = sb.Replace("<span>Cat ", "<span>Cats ");
            sb = sb.Replace("<span>Clear ", "<span>Clears ");
            sb = sb.Replace("<span>Dog ", "<span>Dogs ");
            sb = sb.Replace("<span>Draw ", "<span>Draws ");
            return sb.ToString();
        }
    }
}