﻿using Steamworks;

public class WorkshopQueryAll : WorkshopQueryUGC {

    internal EUGCQuery _queryType;

    internal EUGCMatchingUGCType _fileType;

    public bool matchAnyTag { get; set; }

    public string searchText { get; set; }

    public uint trendRankDays { get; set; }

    internal WorkshopQueryAll(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingUGCTypeFileType) {
        matchAnyTag = false;
        searchText = null;
        trendRankDays = 0;

        _queryType = eQueryType;
        _fileType = eMatchingUGCTypeFileType;
    }

    internal override unsafe void Create() {
        _handle = SteamUGC.CreateQueryAllUGCRequest(_queryType, _fileType, (AppId_t)312530, (AppId_t)312530, _page);
    }

    internal override unsafe void SetQueryData() {
        base.SetQueryData();

        SteamUGC.SetMatchAnyTag(_handle, matchAnyTag);
        SteamUGC.SetSearchText(_handle, searchText);

        if (trendRankDays != 0)
            SteamUGC.SetRankedByTrendDays(_handle, trendRankDays);
    }

}
