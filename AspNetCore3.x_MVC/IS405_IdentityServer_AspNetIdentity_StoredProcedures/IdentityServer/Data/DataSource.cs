using Dapper;
using FastMember;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data.Utils
{
    // This wraps the ORM (DapperHelper -> Dapper) so that you could replace the ORM any time you want and never
    // have dependencies directly on your Stores OR Repositories.

    // Furthermore this language much more closely aligns with developer concepts.
    // Get vs. Query for example.
    // Update vs. Execute Non Scalar for example.

    // This is purely cosmetic and not entirely needed.
    public static class DataSource
    {
        public static async Task<TOut> GetAsync<TIn, TOut>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new() where TOut : class, new()
        {
            return await DapperHelper
                .QuerySingleAsync<TOut>(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task<TOut> GetValueAsync<TOut>(string connectionString, string storedProcedureName, params object[] parameters) where TOut : struct
        {
            if (parameters.Length == 0) { parameters = null; }

            return await DapperHelper
                .QuerySingleAsync<TOut>(connectionString, storedProcedureName, parms: parameters)
                .ConfigureAwait(false);
        }

        public static async Task<TOut> GetAsync<TOut>(string connectionString, string storedProcedureName, params object[] parameters) where TOut : class, new()
        {
            if (parameters.Length == 0) { parameters = null; }

            return await DapperHelper
                .QuerySingleAsync<TOut>(connectionString, storedProcedureName, parms: parameters)
                .ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TOut>> GetManyAsync<TIn, TOut>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new() where TOut : new()
        {
            return await DapperHelper
                .QueryAsync<TOut>(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TOut>> GetManyAsync<TOut>(string connectionString, string storedProcedureName, params object[] parameters) where TOut : class, new()
        {
            if (parameters.Length == 0) { parameters = null; }

            return await DapperHelper
                .QueryAsync<TOut>(connectionString, storedProcedureName, parms: parameters)
                .ConfigureAwait(false);
        }

        public static async Task<TOut> InsertAndReturnValueAsync<TIn, TOut>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new() where TOut : struct
        {
            return await DapperHelper
                .ExecuteScalarAsync<TOut>(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task<string> InsertAndReturnValueAsync<TIn>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new()
        {
            return await DapperHelper
                .ExecuteScalarAsync<string>(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task InsertAsync<TIn>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new()
        {
            await DapperHelper
                .ExecuteAsync(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task ExecuteAsync<TIn>(TIn input, string connectionString, string storedProcedureName)
        {
            await DapperHelper
                .ExecuteAsync(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task UpdateAsync<TIn>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new()
        {
            await DapperHelper
                .ExecuteAsync(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task UpsertAsync<TIn>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new()
        {
            await DapperHelper
                .ExecuteAsync(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        public static async Task<TOut> UpsertAndReturnValueAsync<TIn, TOut>(TIn input, string connectionString, string storedProcedureName) where TIn : class, new() where TOut : struct
        {
            return await DapperHelper
                .ExecuteScalarAsync<TOut>(connectionString, storedProcedureName, dynamicParameters: ConstructDynamicParameters(input))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Uses FastMember (reflection with in-memory caching) to look at object properties/values and construct SQL Parameters.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        public static DynamicParameters ConstructDynamicParameters<TIn>(TIn input)
        {
            var objectMemberAccessor = TypeAccessor.Create(input.GetType());

            return new DynamicParameters(
                objectMemberAccessor
                    .GetMembers()
                    .ToDictionary(x => x.Name, x => objectMemberAccessor[input, x.Name]));
        }
    }
}
