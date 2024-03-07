# UniFramework.Log

一个轻量级的UIPanel池。

```
public void OnEnter()
{
    var package = YooAssets.GetPackage("main");

    objCanval = package.LoadAssetSync<GameObject>("Canvas");
    objSplash = package.LoadAssetSync<GameObject>("UISplash"); 

    GameObject desk = GameObject.Instantiate((GameObject)objCanval.AssetObject);

    //使用 Canvas 面板，初始化一个 PanelPool
    PanelPool panelPool = PanelPool.Initalize(desk);

    //向 PanelPool 的Close容器内实例化一个 UISplash 页面，并显示
    panelPool.AddPanel<UISplash>((GameObject)objSplash.AssetObject).Show();

    GameManager.PanelPool = panelPool;
}

public void OnExit()
{
    objCanval.Dispose();
    objSplash.Dispose();
}
```