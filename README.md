

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

