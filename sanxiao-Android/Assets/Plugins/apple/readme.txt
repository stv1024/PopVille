由于unity3D_4.3 采取了不同的编译方式，所以不能像3.5.6那样子直接将.mm文件放进Assets/Plugins/iOS中，
应该先编译好。然后再按如下步骤进行。
1.将该文件下得UnityAppController.h和UnityAppController.mm文件复制到xCodeProject工程的Classs文件夹下，覆盖原来的两个文件。


2.将其余的文件，包括图片资源复制到xCodeProject工程的Libraries目录下。

原因：
1.如果还像3.5.6那样子采用放在iOS文件夹下，编译出来的xCodeProject将不会覆盖掉UnityAppController.*文件。
2.而且会导致xCodeProject出现重复编译的错误。
