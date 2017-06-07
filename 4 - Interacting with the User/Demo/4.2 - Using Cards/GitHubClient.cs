using Octokit;
using System;
using System.Threading.Tasks;
using gh = Octokit;

namespace Bot_Application
{
    public static class GitHubClient
    {
        private static readonly Lazy<gh.GitHubClient> _client = new Lazy<gh.GitHubClient>(() => new gh.GitHubClient(new ProductHeaderValue(@"MyBotApp")));

        public static Task<gh.User> LoadProfile(string username) => _client.Value.User.Get(username);

        public static Task<gh.SearchUsersResult> ExecuteSearch(string query) => _client.Value.Search.SearchUsers(new SearchUsersRequest(query));
    }
}