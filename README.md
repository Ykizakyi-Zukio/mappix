C# .NET open-source obfuscator. Mappix

Code requirements:
- Supports any .NET Framework C# Library of input code

Requirements:
- Require .NET Framework 6 or never
- Require OS's: Windows, Linux, MacOS

Tools:
- Remove all unusual symbols (\t\r)
- Remove all basic comments (//)
- Remove all region comments (*//*)
- Remove all Visual Studio regions
- Rename a local/public classes

Mappix can to remove all extra content from code (return symbols, comments, regions etc.),
and made a source code is unreadable and incomprehensible.
Code performance remains the same and does not decrease, and the data size may decrease due to the removal of comments, tabs, spaces, line breaks and data renaming.
Obfuscation uses basic text processing functions and does not use heavy third-party libraries.
