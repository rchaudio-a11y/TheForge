' **Character Count:** TBD
' **Document Type:** Code
' **Created:** 2024-11-01
' **Last Updated:** 2025-01-02
' **Related:** IModule.vb

Namespace Modules.Interfaces
    ''' <summary>
    ''' Interface for module configuration.
    ''' </summary>
    Public Interface IModuleConfiguration
        ''' <summary>
        ''' Gets the raw configuration data dictionary.
        ''' </summary>
        ReadOnly Property ConfigurationData As Dictionary(Of String, String)

        ''' <summary>
        ''' Gets a configuration value by key.
        ''' </summary>
        ''' <param name="key">The configuration key.</param>
        ''' <returns>The configuration value, or empty string if key not found.</returns>
        Function GetValue(key As String) As String

        ''' <summary>
        ''' Sets a configuration value.
        ''' </summary>
        ''' <param name="key">The configuration key.</param>
        ''' <param name="value">The configuration value.</param>
        Sub SetValue(key As String, value As String)

        ''' <summary>
        ''' Checks if a configuration key exists.
        ''' </summary>
        ''' <param name="key">The configuration key to check.</param>
        ''' <returns>True if the key exists, otherwise false.</returns>
        Function HasKey(key As String) As Boolean
    End Interface
End Namespace
