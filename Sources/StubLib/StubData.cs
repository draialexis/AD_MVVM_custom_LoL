using Model;

namespace StubLib;
public partial class StubData : IDataManager
{
    public StubData()
    {
        ChampionsMgr = new ChampionsManager(this);
        SkinsMgr = new SkinsManager(this);
        RunesMgr = new RunesManager(this);
        RunePagesMgr = new RunePagesManager(this);

        InitSkins();
        InitRunePages();
    }

    public IChampionsManager ChampionsMgr { get; }

    public ISkinsManager SkinsMgr { get; }

    public IRunesManager RunesMgr { get; }

    public IRunePagesManager RunePagesMgr { get; }

    private List<Tuple<Champion, RunePage>> championsAndRunePages = new();

    private void InitChampionsAndRunePages()
    {
        championsAndRunePages.Add(Tuple.Create(champions[0], runePages[0]));    
    }


    
}

