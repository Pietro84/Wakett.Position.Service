using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;

namespace Wakett.Position.Service.Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly string _connectionString;

        public PositionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<FinancialPosition>> GetPositionsByInstrumentIdAsync(string instrumentId)
        {
            var positions = new List<FinancialPosition>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT InstrumentId, Quantity, InitialRate, Side, ProfitLoss FROM FinancialPositions WHERE InstrumentId = @InstrumentId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InstrumentId", instrumentId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var position = new FinancialPosition
                            {
                                InstrumentId = reader.GetString(0),
                                Quantity = reader.GetDecimal(1),
                                InitialRate = reader.GetDecimal(2),
                                Side = reader.GetInt32(3),
                                ProfitLoss = reader.GetDecimal(4)
                            };

                            positions.Add(position);
                        }
                    }
                }
                return positions;
            }
        }

        public async Task UpdatePositionProfitLossAsync(FinancialPosition position)
        {
            string updateQuery = @"UPDATE FinancialPositions SET ProfitLoss = @ProfitLoss WHERE InstrumentId = @InstrumentId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProfitLoss", position.ProfitLoss);
                    command.Parameters.AddWithValue("@InstrumentId", position.InstrumentId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
