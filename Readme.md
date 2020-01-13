# License Generator

A cross-platform .NET solution for licensing .NET applications with license files. The solution consists of a class library for all license related functions, a license generator application, and a sandbox project.

The License system is based on asymmetric encryption, using a pair of private and public keys to sign license files. `System.Security.RSACryptoServiceProvider` is used.

Licenses are generated based on hardware particulars; a machine ID is generated for each machine. The machine ID is then signed using a private key. The resulting signature and the machine ID are stored in a file, which is essentially the generated license file. This license file can then be placed on a client machine for license verification.

###### License class library:

The `AppLicense` class library contains all the components to generate license files and to verify them. The library is built on .NET Standard 2.0, particularly for compatibility with .NET Framework.

This library can be referenced/included in applications to validate generated license files. To generate license files, use the included license generator application, or build your own implementation.

###### License generator application:

The License Generator application is a WPF application based on .NET Core 3.0. The application is merely a UI wrapper around the license generation portion of `AppLicense` library.

The application takes an input of any string (like the machine ID), and an XML string of the private key (generated using `RSACryptoServiceProvider.ToXmlString`). The license file containing the machine ID and the generated signature can then be generated.

A sample for license validation can be found in the sandbox project.

Roadmap: Include a feature to generate asymmetric key pairs.

###### License sandbox:

The sandbox project references the `AppLicense` library and showcases the usage for generating and validating license files

### Notes:

The `AppLicense` library is a crude implementation and may contain vulnerabilities.

### License:

Licensed under the MIT License
