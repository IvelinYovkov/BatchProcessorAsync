using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NightlyDeploys
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var jobNumber = 13;
            var parallelLevelOfChildJobs = 5;

            var builds = InitialLoading(parallelLevelOfChildJobs);
            int buildsInList = parallelLevelOfChildJobs;

            Stopwatch sw = Stopwatch.StartNew();

            await RunBuildsAsync(jobNumber, builds, buildsInList);

            Console.WriteLine($"Total processing time: {sw.ElapsedMilliseconds}");
            sw.Stop();
        }

        private static async Task RunBuildsAsync(int jobNumber, List<Task<string>> builds, int buildsInList)
        {
            while (builds.Any())
            {                
                var build = await Task.WhenAny(builds);
                string result = await build;

                Console.WriteLine(result);
                builds.Remove(build);

                if (buildsInList < jobNumber)
                {
                    builds.Add(BuildArtefactAssync());
                    buildsInList++;
                }
            }
        }

        static List<Task<string>> InitialLoading(int max = 1)
        {
            var builds = new List<Task<string>>();

            for (int cntr = 0; cntr < max; cntr++)
            {
                builds.Add(BuildArtefactAssync());
            }

            return builds;
        }

        static async Task<string> BuildArtefactAssync()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:52174/BuildArtefact");
            request.Timeout = 600000;

            using HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
            using Stream responseStream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(responseStream);
            return await reader.ReadToEndAsync();
        }
    }
}
