using Model;

namespace StubLib;
public partial class StubData : IDataManager
{
    public StubData()
    {
        ChampionsMgr = new ChampionsManager(this);
        SkinsMgr = new SkinsManager(this);

        InitSkins();
    }

    public IChampionsManager ChampionsMgr { get; }

    public ISkinsManager SkinsMgr { get; }

    private List<Champion> championsPages = new();

    private void InitChampionsPages()
    {
        championsPages.Add(champions[0]);
    }

}
