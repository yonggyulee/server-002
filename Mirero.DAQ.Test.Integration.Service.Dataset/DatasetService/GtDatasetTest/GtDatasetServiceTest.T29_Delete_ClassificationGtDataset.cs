﻿using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T29_Delete_ClassificationGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}