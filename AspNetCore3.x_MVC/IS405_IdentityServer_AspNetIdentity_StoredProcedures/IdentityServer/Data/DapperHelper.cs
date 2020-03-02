using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using static Dapper.SqlMapper;

namespace IdentityServer.Data.Utils
{
    // This is just something I always have laying around.

    // Demonstrates a variety of use cases/examples.
    public static class DapperHelper
    {
        public static T QueryFirst<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.QueryFirstOrDefault<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> QueryFirstAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static T QuerySingle<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.QuerySingleOrDefault<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> QuerySingleAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QuerySingleOrDefaultAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static async Task<T> QuerySingleAsync<T>(string connectionString, string storedProc, DynamicParameters dynamicParameters)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QuerySingleOrDefaultAsync<T>(storedProc, dynamicParameters, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static IEnumerable<T> Query<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.Query<T>(storedProc, parms, null, false, 60, commandType: CommandType.StoredProcedure);
        }

        public static IEnumerable<T> Query<T>(string connectionString, string storedProc, DynamicParameters dynamicParameters)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.Query<T>(storedProc, dynamicParameters, null, false, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string storedProc, DynamicParameters dynamicParameters)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<T>(storedProc, dynamicParameters, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static GridReader QueryMultiple(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.QueryMultiple(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task<GridReader> QueryMultipleAsync(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryMultipleAsync(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static void Execute(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            connection.Execute(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task ExecuteAsync(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static async Task ExecuteAsync(string connectionString, string storedProc, DynamicParameters dynamicParameters)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(storedProc, dynamicParameters, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static T ExecuteScalar<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return connection.ExecuteScalar<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> ExecuteScalarAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.ExecuteScalarAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static async Task<T> ExecuteScalarAsync<T>(string connectionString, string storedProc, DynamicParameters dynamicParameters)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.ExecuteScalarAsync<T>(storedProc, dynamicParameters, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public static T TransQueryFirst<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            var result = connection.QueryFirstOrDefault<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();

            return result;
        }

        public static async Task<T> TransQueryFirstAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryFirstOrDefaultAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();

            return result;
        }

        public static T TransQuerySingle<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            var result = connection.QuerySingleOrDefault<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();

            return result;
        }

        public static async Task<T> TransQuerySingleAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QuerySingleOrDefaultAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();

            return result;
        }

        public static IEnumerable<T> TransQuery<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            var result = connection.Query<T>(storedProc, parms, null, false, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();

            return result;
        }

        public static async Task<IEnumerable<T>> TransQueryAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();

            return result;
        }

        public static GridReader TransQueryMultiple(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            var gridReader = connection.QueryMultiple(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();

            return gridReader;
        }

        public static async Task<GridReader> TransQueryMultipleAsync(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            var gridReader = await connection.QueryMultipleAsync(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();

            return gridReader;
        }

        public static void TransExecute(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            connection.Execute(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();
        }

        public static async Task TransExecuteAsync(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();
        }

        public static T TransExecuteScalar<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope();
            using var connection = new SqlConnection(connectionString);

            var result = connection.ExecuteScalar<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure);
            transactionScope.Complete();

            return result;
        }

        public static async Task<T> TransExecuteScalarAsync<T>(string connectionString, string storedProc, params object[] parms)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var connection = new SqlConnection(connectionString);

            var result = await connection.ExecuteScalarAsync<T>(storedProc, parms, null, 60, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionScope.Complete();

            return result;
        }
    }
}
