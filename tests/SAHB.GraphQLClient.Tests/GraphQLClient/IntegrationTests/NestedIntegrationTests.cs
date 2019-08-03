﻿using System.Threading.Tasks;
using Newtonsoft.Json;
using SAHB.GraphQLClient.FieldBuilder;
using SAHB.GraphQLClient.QueryGenerator;
using SAHB.GraphQLClient.Extentions;
using Xunit;
using SAHB.GraphQLClient.Deserialization;

namespace SAHB.GraphQLClient.Tests.GraphQLClient.IntegrationTests
{
    public class NestedIntegrationTests
    {
        private readonly IGraphQLQueryGeneratorFromFields _queryGenerator;
        private readonly IGraphQLFieldBuilder _fieldBuilder;
        private readonly IGraphQLDeserialization _deserilization;

        public NestedIntegrationTests()
        {
            _queryGenerator = new GraphQLQueryGeneratorFromFields();
            _fieldBuilder = new GraphQLFieldBuilder();
            _deserilization = new GraphQLDeserilization();
        }

        [Fact]
        public async Task TestGraphQLClient()
        {
            var responseContent = "{\"data\":{\"Me\":{\"Firstname\":\"Søren\", Age:\"24\", \"lastname\": \"Bjergmark\"}}}";
            var httpClient = new HttpClientMock.GraphQLHttpExecutorMock(responseContent, "{\"query\":\"query{me{firstname age lastname}}\"}");
            var client = new GraphQLHttpClient(httpClient, _fieldBuilder, _queryGenerator, _deserilization);

            // Act
            var response = await client.Query<QueryToTest>("");

            // Assert
            Assert.Equal("Søren", response.Me.Firstname);
            Assert.Equal("Bjergmark", response.Me.lastname);
            Assert.Equal(24u, response.Me.Age);
        }

        public class QueryToTest
        {
            public Person Me { get; set; }
        }

        public class Person
        {
            public string Firstname { get; set; }
            public uint Age { get; set; }
            public string lastname { get; set; }
        }
    }
}
