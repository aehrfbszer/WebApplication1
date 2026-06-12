MyMinimalApi/
│
├── Program.cs                 # 唯一的入口：配置中间件、DI 容器、启动服务
├── MyMinimalApi.csproj        # 项目配置文件（类似 package.json 或 Cargo.toml）
│
├── 📁 Common/                 # 1. 全局通用基础设施
│   ├── 📁 Behaviours/         #    - 验证拦截器等
│   ├── 📁 Exceptions/         #    - 自定义业务异常
│   └── 📁 Extensions/         #    - 各种框架扩展方法
│
├── 📁 Data/                   # 2. 数据持久化层 (EF Core)
│   ├── AppDbContext.cs        #    - 数据库上下文
│   └── 📁 Migrations/         #    - EF Core 自动生成的数据库迁移脚本
│
├── 📁 Domain/                 # 3. 领域模型层 (Entity)
│   ├── User.cs                #    - 纯粹的数据库实体模型
│   └── Product.cs             #    - 实体模型
│
└── 📁 Features/               # 4. 核心业务层 (垂直切片：核心秘密所在)
    ├── 📁 Users/              #    - 用户模块所有内聚代码
    │   ├── CreateUser.cs      #      👉 高度内聚：包含该接口的 Request, Response, Validator, Endpoint
    │   ├── GetUser.cs         #      👉 获取单个用户接口
    │   └── UserModule.cs      #      👉 用户模块的路由注册中心
    │
    └── 📁 Products/           #    - 产品模块
        ├── CreateProduct.cs
        └── ProductModule.cs
