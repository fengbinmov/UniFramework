# UniFramework.Log

一个轻量级的日志本地存储。


```
//初始化日志记录
UniLog.Initalize();

//使用手动驱动方式进行每帧记录(否则将会在每次日志输出时进行存储)
UniLogDriver.Initalize();
```

```
//销毁日志记录
UniLog.Destroy();
```