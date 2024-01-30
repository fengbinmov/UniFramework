# UniFramework.Singleton

一个轻量级的单例系统。



- 对于继承 SingletonInstance 类的对象来说，将直接可通过 “<类名>.Instance ”的方式获取对象索引；

- 可在继承对象中选择实现，如下三个事件接收方法。(会在 UniSingleton 通过委托广播，不实现无消耗)

```c#
//    /// <summary>
//    /// 创建单例
//    /// </summary>
//    void OnCreate(System.Object createParam);

//    /// <summary>
//    /// 更新单例
//    /// </summary>
//    void OnUpdate();

//    /// <summary>
//    /// 销毁单例
//    /// </summary>
//    void OnDestroy();
```
