﻿// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.Domains;

namespace Framework.Permissions.Entities;

public static class PermissionGrantRecordConstants
{
    public const int NameMaxLength = 128;
    public const int ProviderNameMaxLength = 64;
    public const int ProviderKeyMaxLength = 64;
    public const int TenantIdMaxLength = DomainConstants.IdMaxLength;
}
