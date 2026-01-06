''' ============================================================================
''' File: RunT058Tests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Test runner for T058 migration tests
''' Created: 2026-01-05
''' ============================================================================

Imports RCH.TemplateStorage.Testing

Module RunT058Tests

    Sub Main()
        Console.WriteLine("RCH.TemplateStorage - T058 Migration Test Runner")
        Console.WriteLine()

        Try
            T058_MigrationTests.RunAllTests()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"Fatal Error: {ex.Message}")
            Console.WriteLine(ex.StackTrace)
            Console.ResetColor()
        End Try

        Console.WriteLine()
        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub

End Module
