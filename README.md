### Oculus Integration インストール

- Asset Store からインストールする

### Oculus Integration を Unity に設定する

- Default の camera を Unity から削除する
- OVRCameraRig(OVRPlayerController からでも OK) を Hierarchy にコピーする
- OVRCameraRig の OVR Manager -> Tracking Origin Type を Floor Level にする

### Oculus App 設定

- Unity の Oculus -> Platform -> Edit Settings を開く
- Create / Find your App App から Oculus Web に行って App を作成して、アプリ ID を取得する
- Unity に戻って Oculus Rift と Oculus Quest にアプリ ID を入力する
- Unity の Oculus -> Avatar -> Edit Settings を開く
- Oculus Rift と Oculus Quest にアプリ ID を入力する

### Project Build 設定

- Unity の File -> Build Settings を開く
- Scene を追加する
- Platform を Android にして、Switch Platform する
- Player Settings を開く
- XR Plugin Management から Install XR Plugin Management を実行する
- Plug-in Providers の Oculus をクリックする
- Player から Other Settings -> Rendering -> Color Space を Linear にする
- Identification -> Package Name にパッケージネーム(com.example.appname)を入力する
- Identification -> Minimum API Level を Android 7.0 (API Level 24)にする
- Build Settings に戻って、Textture Compression を ASTC にする
- Run Device を確認して、Build And Run する

### Oculus Touch Controller を VR app 上に表示させる

- OVRCameraRig の OVR Manager の Quest Features の Hand Tracking Support が Controllers Only になっていることを確認する
- Oculus Touch などのコントローラや手の Prefab を Left/RightControllerAnchor 配下 or 並列に配置する
- 配置した Prefab の位置をリセットする
- Oculus Touch に Rigidbody を追加して、Mass を 10、Use Gravity を Off、Is Kinematic を On、Colision Detection を Continuous Dynamic にする
- Oculus Touch に Capsule Colider を追加する

### Hand Tracking を有効にして VR app 上に表示させる

- OVRCameraRig の OVR Manager の Quest Features の Hand Tracking Support が Controllers And Hands にする
- OVRHandPrefab を Left/RightControllerAnchor 配下 or 並列に配置する
- OVRHandPrefab の OVR Hand、OVR Skelton、OVR Mesh の Type を Hand Left/Right にする
- OVRHandPrefab の OVR Skelton の Enable Physics Capsules を有効にする

### Blender からインポートした fbx ファイルを Unity にエクスポートする

- Assets にフォルダを作ってそこにドラッグアンドドロップする
- Hierarchy にそれを配置する
- マテリアルがないので配置したファイルにマテリアルとしてテクスチャなどをアタッチする

### 物体を Controller でつかめるようにする

- 物体に Rigidbody をアタッチする(Use Gravity を有効化する)
- 物体に Box Collider をアタッチする(重力がある場合に、衝突させないと地面をすり抜けてしまうため)
- 物体に OVR Grabbable をアタッチする
- OVRCameraRig -> TrackingSpace -> Left/RightHandAnchor の直下のコントローラや手の Prefab の OVR Grabber の Parent Held Object を有効化する(変な位置で掴まないようにするため)

### Controller の Joystrick による移動

- Hierarchy View に OVRPlayerController を配置する
- 地面に Box Collider をアタッチする
- OVRPlayerController の OVRCameraRig の Tracking の Tracking Origin Type を Floor Level にする

### 物体を Hand Tracking でつかめるようにする

- Left/RightControllerAnchor 配下に Sphere オブジェクトを配置する
- Sphere オブジェクトに [C# スクリプト](/Assets/Scripts/Grab.cs)を配置する
  - スクリプトパラメータは OVRHand と OVRSkelton、GameObject(Sphere)
  - Update メソッドでは Sphere の Position、Rotate を人差し指の先に移動させる
  - OnTriggerStay メソッドでは親指の Pinch Strength が強くなると Trigger 領域に入った Object を Sphere の子供にして Position をリセット、RigidBody の isKinematic を true、にする
- Sphere の Collider の Is Trigger を有効化する

### CanvasWithDebug を VR App 上に表示させる

- Assets/Oculus 内の CanvasWithDebug を Hierarchy にドラッグ&ドロップする
- CanvasWithDebug の Canvas の Render Mode を World Space にする
- CanvasWithDebug の Canvas の Event Camera に OVRCameraRig の CenterEyeAnchor を設定する
- CanvasWithDebug の Pos や Width/Height を調整する
- [C# スクリプト](/Assets/Scripts/Grab.cs)からログを出力する
- [参考 URL](https://joyplot.com/documents/oculus-unity-debuglog-display/)

### コントローラのボタン押下でメニューを表示させる

- メニュー用のプレハブを作成する
  - UI -> Canvas を作成して、Canvas -> Render Mode を Screen Space - Camera にして、Render Camera に OVRCameraRig -> TrackingSpace -> CenterEyeAnchor を設定する
  - Canvas に[C# スクリプト](/Assets/Scripts/ResetButton.cs)をアタッチする
    - このスクリプトにはボタン押下時の挙動(例: シーン再読み込み)が書かれている
  - Canvas の下に UI -> Image と UI -> Button -TextMeshPro を作成する
  - Button の Button -> On Click に[C# スクリプト](/Assets/Scripts/ResetButton.cs)内の関数を設定する
  - Project View 内の UIHelpers を Hierarchy View に配置する
  - UIHelpers -> EventSystem の OVR Input Module -> Ray Transform に OVRCameraRig -> TrackingSpace -> RightHandAnchor を設定する

### 指鉄炮

- [Hand Tracking を有効にして VR app 上に表示させる](# "Hand Tracking を有効にして VR app 上に表示させる")から Hand Tracking 状態にする
- OVRHandPrefab 配下に Sphere オブジェクトを配置する
- この Sphere オブジェクト用の Material を Rendering Mode を Cutout で Albedo の A を 0 にして透明にして作成してアタッチする
- この Sphere に [C# スクリプト](/Assets/Scripts/Throw.cs)と[C# スクリプト](/
  Assets/Scripts/Lifetime.cs)をアタッチする
- この Sphere を Project View に配置して Prefab 化する

### OVRPlayerController の手や顔を PUN2 で共有する

- ネットワークオブジェクトを制御するコントローラを GameObject を作成する
- このコントローラに Photon View をアタッチする
- このコントローラに Photon Transform View をアタッチして Use Local を Off にする
- このコントローラの Photon View の Observed Component にこのコントローラの Photon Transform View をアタッチする
- このコンローラの配下に顔と手の GameObject を配置する
- 顔と手の GameObject に Photon View をアタッチする
- 顔と手の GameObject に Photon Transform View をアタッチして Use Local を Off にする
- 顔と手の GameObject の Photon View の Observed Component に自分自身(顔 or 手)の Photon Transform View をアタッチする
- このコントローラに [C# スクリプト](/Assets/Scripts/NetworkObjectsController.cs)をアタッチしてパラメータに顔と手を設定する
- このコントローラを Project View に配置して Prefab 化する
