using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Data;

public static class SeedData
{
    public static void SeedLicenses(ApplicationDbContext context)
    {
        if (context.Licenses.Any()) return;

        var licenses = new[]
        {
            new SoftwareLicense
            {
                Name = "MIT License",
                Abbreviation = "MIT",
                LicenseUrl = "https://opensource.org/licenses/MIT",
                Type = LicenseType.OpenSource,
                IsCopyleft = false,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "A permissive license that allows for commercial use, modification, distribution, and private use. Requires attribution."
            },
            new SoftwareLicense
            {
                Name = "Apache License 2.0",
                Abbreviation = "Apache-2.0",
                LicenseUrl = "https://opensource.org/licenses/Apache-2.0",
                Type = LicenseType.OpenSource,
                IsCopyleft = false,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "Similar to MIT but provides patent protection and requires notice of changes made to the code."
            },
            new SoftwareLicense
            {
                Name = "GNU General Public License v3.0",
                Abbreviation = "GPL-3.0",
                LicenseUrl = "https://opensource.org/licenses/GPL-3.0",
                Type = LicenseType.OpenSource,
                IsCopyleft = true,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = true,
                CompatibilityLevel = CompatibilityLevel.Low,
                Description = "Strong copyleft license that requires derivative works to be distributed under the same license."
            },
            new SoftwareLicense
            {
                Name = "GNU General Public License v2.0",
                Abbreviation = "GPL-2.0",
                LicenseUrl = "https://opensource.org/licenses/GPL-2.0",
                Type = LicenseType.OpenSource,
                IsCopyleft = true,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = true,
                CompatibilityLevel = CompatibilityLevel.Low,
                Description = "Earlier version of GPL with strong copyleft requirements."
            },
            new SoftwareLicense
            {
                Name = "GNU Lesser General Public License v3.0",
                Abbreviation = "LGPL-3.0",
                LicenseUrl = "https://opensource.org/licenses/LGPL-3.0",
                Type = LicenseType.OpenSource,
                IsCopyleft = true,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = true,
                CompatibilityLevel = CompatibilityLevel.Medium,
                Description = "Weaker copyleft license that allows linking with proprietary software."
            },
            new SoftwareLicense
            {
                Name = "BSD 3-Clause License",
                Abbreviation = "BSD-3-Clause",
                LicenseUrl = "https://opensource.org/licenses/BSD-3-Clause",
                Type = LicenseType.OpenSource,
                IsCopyleft = false,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "Permissive license similar to MIT but with an additional clause prohibiting use of contributor names for endorsement."
            },
            new SoftwareLicense
            {
                Name = "BSD 2-Clause License",
                Abbreviation = "BSD-2-Clause",
                LicenseUrl = "https://opensource.org/licenses/BSD-2-Clause",
                Type = LicenseType.OpenSource,
                IsCopyleft = false,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "Simplified BSD license without the endorsement clause."
            },
            new SoftwareLicense
            {
                Name = "Mozilla Public License 2.0",
                Abbreviation = "MPL-2.0",
                LicenseUrl = "https://opensource.org/licenses/MPL-2.0",
                Type = LicenseType.OpenSource,
                IsCopyleft = true,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = true,
                CompatibilityLevel = CompatibilityLevel.Medium,
                Description = "File-level copyleft license that allows combining with proprietary code."
            },
            new SoftwareLicense
            {
                Name = "ISC License",
                Abbreviation = "ISC",
                LicenseUrl = "https://opensource.org/licenses/ISC",
                Type = LicenseType.OpenSource,
                IsCopyleft = false,
                IsOsiApproved = true,
                RequiresAttribution = true,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "Simplified version of the BSD license with equivalent functionality."
            },
            new SoftwareLicense
            {
                Name = "Creative Commons Zero v1.0 Universal",
                Abbreviation = "CC0-1.0",
                LicenseUrl = "https://creativecommons.org/publicdomain/zero/1.0/",
                Type = LicenseType.PublicDomain,
                IsCopyleft = false,
                IsOsiApproved = false,
                RequiresAttribution = false,
                RequiresSourceDisclosure = false,
                CompatibilityLevel = CompatibilityLevel.High,
                Description = "Public domain dedication allowing unrestricted use without any requirements."
            }
        };

        context.Licenses.AddRange(licenses);
        context.SaveChanges();
    }
}