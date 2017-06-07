using Octokit;
using System;
using System.Threading.Tasks;
using gh = Octokit;

namespace Bot_Application
{
    public class GitHubClient
    {
        private readonly Lazy<gh.GitHubClient> _client;
        public GitHubClient(string appName = @"MyBotApp")
        {
            _client = new Lazy<gh.GitHubClient>(() => new gh.GitHubClient(new ProductHeaderValue(appName)));
        }

        public Task<gh.User> LoadProfile(string username) => _client.Value.User.Get(username);

        public Task<gh.SearchUsersResult> ExecuteSearch(string query) => _client.Value.Search.SearchUsers(new SearchUsersRequest(query));
    }
}