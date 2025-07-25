﻿using Framework.Sql;
using Framework.Sql.Sqlite;

namespace Tests;

public sealed class SqliteConnectionFactoryTests : SqlConnectionFactoryTestBase
{
    public override string GetConnection()
    {
        return "DataSource=:memory:";
    }

    public override ISqlConnectionFactory GetFactory()
    {
        return new SqliteConnectionFactory(GetConnection());
    }

    [Fact]
    public override Task should_return_connection_string()
    {
        return base.should_return_connection_string();
    }

    [Fact]
    public override Task should_create_new_connection()
    {
        return base.should_create_new_connection();
    }

    [Fact]
    public override Task should_get_open_connection()
    {
        return base.should_get_open_connection();
    }

    [Fact]
    public override Task should_dispose_connection()
    {
        return base.should_dispose_connection();
    }

    [Fact]
    public override Task should_get_open_connection_concurrently()
    {
        return base.should_get_open_connection_concurrently();
    }

    [Fact]
    public async Task should_execute_sql_command()
    {
        // given
        await using var sut = GetFactory();
        var connection = await sut.GetOpenConnectionAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE test (id INTEGER PRIMARY KEY, name TEXT)";
        await command.ExecuteNonQueryAsync();

        // when
        command.CommandText = "INSERT INTO test (name) VALUES ('test')";
        await command.ExecuteNonQueryAsync();

        // then
        command.CommandText = "SELECT COUNT(*) FROM test";
        var result = await command.ExecuteScalarAsync();
        result.Should().Be(1);
    }
}
