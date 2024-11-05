var build = new ApplicationBuilder();

build.Use(next =>
{
    return async context =>
    {
        Console.WriteLine("第一个中间件开始");
        await next(context);
        Console.WriteLine("第一个中间件结束");
    };
});

build.Use(next =>
{
    return async context =>
    {
        Console.WriteLine("第二个中间件开始");
        await next(context);
        Console.WriteLine("第二个中间件结束");
    };
});

build.Use(next =>
{
    return async context =>
    {
        Console.WriteLine("执行最后一个中间件");
    };
});

var app = build.Build();
app(new HttpContext());


public class HttpContext
{

}

public delegate Task RequestDelegate(HttpContext context);

public class ApplicationBuilder
{
    private List<Func<RequestDelegate, RequestDelegate>> _componments = new();

    public void Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        _componments.Add(middleware);
    }

    public RequestDelegate Build()
    {
        RequestDelegate app = context =>
        {
            throw new InvalidOperationException();
        };

        for (int i = _componments.Count - 1; i > -1; i--)
        {
            app = _componments[i](app);
        }

        return app;
    }
}