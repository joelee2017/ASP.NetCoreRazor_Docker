

# ASP.NetCoreRazor_Docker

Docker練習

參考源：深入浅出 ASP.NET Core 与 Docker 入门课程目录

建議配合該視頻影片、圖文進行學習

https://www.52abp.com/yoyomooc/aspnet-core-mvc-in-docker-index

------

##### 一、創建RazorPage示範應用程序

建立一個 Asp.Net Core Razor App

------

##### 二、Docker的鏡像與容器常見面試答疑及命令行操作

Windows  系統需安裝  Docker Desktop for Windows user manual ，並請確認  Hpler-V  已開啟。

Docker Desktop  網址：https://hub.docker.com/editions/community/docker-ce-desktop-windows/

**注意!!** 若是Windows 10 家用版需要透過「腳本」安裝 Hpler-V 在安裝 Docker Desktop。
安裝完後，若是跳出提醒確定一下是否 Linux 核心更新未安裝。

WSL2 Linux 核心更新套件 (適用於 x64 電腦) 網址：https://docs.microsoft.com/zh-tw/windows/wsl/install-win10#step-4---download-the-linux-kernel-update-package

```dockerfile
涉及的命令行
命令 作用
docker image ls 查詢所有的鏡像
docker pull 下載鏡像
docker rmi 刪除鏡像
docker build 創建一個自定義的鏡像
docker create 創建容器
docker ps 查詢所有的容器
docker start 啟動容器
docker stop 停止容器
docker logs 查看容器的運行日誌記錄
docker run 創建並運行一個容器
docker cp 將文件複製到容器中
docker diff 查看容器文件的變化
docker exec 在容器中運行命令
docker commit 將修改的容器創建為鏡像
docker tag 為鏡像分配一個標記
docker login docker logout 從鏡像倉庫中登錄或註銷
docker push 將鏡像發佈到倉庫中
docker inspect 查看容器的詳細配置
docker rmi -f $(docker image ls -aq) 
```

-f 代表強制

-q 代表返回指定鏡像ID

------

##### 三、創建一個自定義ASP.NET Core RazorPage Docker鏡像

創建Dockerfile文件

```powershell
手動自建 Dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "YoYoMooc.ExampleApp.dll"]

預備的應用程序鏡像
dotnet restore ##還原
dotnet publish ##打包
dotnet publish --framework net5.0 --configuration Release --output dist 

創建一個自定義鏡像 (檔案名稱皆為小寫)
docker build  -t 檔名小寫 -f 路徑
docker build  -t razor_docker/exampleapp -f "Razor_Docker/Dockerfile" .

```

`-t`參數:用於標記新的鏡像名稱為  razor_docker/exampleapp。

`-f`參數:指定了創建鏡像的說明文件即Dockerfile的完整名稱。

建立完成後 可使用  `docker image ls` 列出當前所有鏡像。 

本練習是使用 visual studio 補助工具生成 Dockerfile。

dotnet publish 中的 dist 資料夾，若是沒有指定專案路徑下，會與專案sln檔同層之中注意!!

------

##### 四、Docker鏡像創建容器的幾種方法

```powershell
建立容器
docker create -p 3000:80 --name exampleApp3000 razor_docker/exampleapp

查詢所有容器列表 
docker ps -a

命令省略了未運行的容器
docker ps 

查詢不論是否運行的容器
-a 

啟動容器 - 若發生異常請確認port是否被佔用。
docker start exampleApp3000 

啟動所有容器
docker start $(docker ps -aq)

停止容器
docker stop exampleApp3000

停止所有容器
docker stop $(docker ps -q)

不建議使用，會直接將容器刪除
docker kill 檔案名稱

獲取容器輸出日誌
docker logs exampleApp3000

啟動後監控
docker start exampleApp3000 先啟動
docker logs -f exampleApp3000 進入容器
Control+C 來停止顯示輸出信息。而容器不受推出docker logs命令的影響。

使用一個命令創建和啟動容器，合併docker create和docker start命令的效果
docker run -p 5000:80 --name exampleApp5000 razor_docker/exampleapp

Linux或macOS，使用 Control+C 會直接停止容器。Windows 系統，Control+C 單純退出，容器則繼續運行，必須額外使用停止容器命令。

自動刪除容器，會在停止容器後自動刪除
docker run -p 6500:80 --rm --name exampleApp6500 razor_docker/exampleapp
docker stop exampleApp6500
```

docker kill 無法將正在運行中的容器刪除，但會不斷的嘗試關閉動作，進而造成 Deadlock 現象將引發崩潰癱瘓。

------

##### 四、複製文件到正在運行的Docker容器中

至專案修改樣式路徑wwwroot\css\site.css

```css
.text-white{
  color:red !important;
}

.bg-success{
  background-color: rgb(71, 71, 71) !important;
} 
```

修改一個容器，除非在不得以的情況下否則不建議使用!!

```powershell
先啟動
docker start exampleApp3000 exampleApp4000 

修改會履蓋掉原來的資料，成功後無任何訊息。
docker cp .\Razor_Docker\wwwroot\css\site.css exampleApp4000:/app/wwwroot/css/site.css

檢查對容器的修改
docker diff exampleApp4000
```

列出結果標首字母代表意義：
A 表示已將一個文件或文件夾添加到容器中。
C 表示文件或文件夾已被修改。如果是文件夾，表示該文件夾內的文件已被添加或刪除。
D 表示文件或文件夾已從容器中刪除。

```asp.net core razor
css link 中加入 asp-append-version="true" 可對緩存進行破壞。
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true"/>
```

------

##### 五、將正在運行的容器保存為本地Docker

展示修改文件

```powershell
docker exec exampleApp4000 cat /app/wwwroot/css/site.css

容器操作容器，須確保容器是在運行中
docker exec

-it 代表這是一個交付是命令，需要在 powershell 中使用
docker exec -it exampleApp4000 /bin/bash

網絡的問題，導致你無法正常安裝VIM工具
apt-get update
apt-get install vim 確認y

查詢有哪些文件
ls -l

進入vim 文件
vim /app/wwwroot/css/site.css

:q 離開
:wq 保存後離開
exit 離開當前進入容器

將修改後的容器創建成新的鏡像
docker commit exampleApp4000 ltm0203/exampleapp:changed
```

------

##### 六、發布Docker鏡像到Dockhub倉庫

請先建立DockerHub帳號 https://hub.docker.com/

```powershell
建立一個鏡象
docker tag razor_docker/exampleapp:latest ltm0203/exampleapp:unchange

登入docker hub
docker login -u 用户名 -p 密码

推送鏡像到倉庫
docker push ltm0203/exampleapp:changed
docker push ltm0203/exampleapp:unchange

拉取
docker pull ltm0203/exampleapp

若不存在請先推送
docker tag razor_docker/exampleapp:latest ltm0203/exampleapp:latest
docker push ltm0203/exampleapp:latest

使用完後請登出
docker logout
```

------

##### 七、Windows自帶容器支持

切換到Windows容器，通過右鍵Docker圖標進行切換。

Switch to Windows containers...

如果出現切換的情況，大多數情況下重啟電腦可以解決這個問題。



創建一個.NET Core Windows鏡像

```dockerfile
#mcr.microsoft.com/dotnet/core/ 是微軟的官方鏡像庫
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 

COPY dist /app
WORKDIR /app

#容器運行時所提供的服務端口
EXPOSE 80

#ENV 命令表示在容器中設置一個環境變量。
ENV ASPNETCORE_URLS http://+:80

ENTRYPOINT ["dotnet", "YoYoMooc.ExampleApp.dll"]
```

也可以使用vs內建輔助工具建立



創建Windows本地鏡像和容器

創建Windows的鏡像和容器的過程與Linux是相同的，我們首先需要在 檔案 根目錄中，打開終端運行以下命令:

```powershell
dotnet restore ##還原包
dotnet publish --framework netcoreapp3.1 --configuration Release --output dist

docker build . -t docker_razor/exampleapp:windows -f Dockerfile.windows
```

為了區別與Linux容器的區別，我們將鏡像名稱命名為docker_razor/exampleapp:windows，添加一個Windows標記，通過參數 -f 指定docker生成的鏡像文件為Dockerfile.windows

完成後確認 docker image ls

```powershell
確用鏡像

docker run -p 7000:80 --name exampleAppWin yoyomooc/exampleapp:windows
```

檢查windows容器

```powershell
容器所在的虛擬網卡地址獲取
docker inspect exampleAppWin

在Window容器中執行命令
docker exec -it exampleAppWin cmd
```

------

##### 八、Docker中的數據卷(Volume)和網絡(NetWork)介紹

開始正式的網絡和卷的學習之前，保證環境一致性是很重要的事情。首先，切換我們的容器環境為Linux平台。然後刪除當前所有的容器，後面我們會重新創建它們。

```powershell
刪除所有容器
docker rm -f $(docker ps -aq)
```

根目錄中創建一個名為Dockerfile.volumes的文件。

```powershell
FROM alpine:3.9
WORKDIR /data
ENTRYPOINT (test -e message.txt && echo "文件已存在" \
    || (echo "創建文件中..." \
    && echo 你好, Docker 時間： $(date '+%X') > message.txt)) && cat message.txt
```

創建鏡像、運行

```powershell
創立
docker build . -t docker_razor/vtest -f "Razor_Docker/Dockerfile.volumes"

運行
docker run --name vtest docker_razor/vtest

驗證
docker start -a vtest

刪除
docker rm -f vtest
```

------

