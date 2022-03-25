Hello! Thanks for purchasing.

New: video tutorial on how to setup https://www.youtube.com/watch?v=9jULIXmD8kc

For using this package, you need to:

1. Set project to export for Oculus quest (Here's a good video on how to do it: https://www.youtube.com/watch?v=KLMf4yTCc0w)
2. Import Oculus Integration
3. Import TMPro (Text Mesh Pro) if not already imported
4. Import Photon Pun2 free version https://assetstore.unity.com/packages/tools/network/pun-2-free-119922
5. Import Photon Voice 2 free https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518
6. Go to https://www.photonengine.com/ and create an account, and create a new PUN app and you will get an appID. Copy it and paste it in the PhotonServerSettings config file in the project in App id Realtime and App id Voice fields.
7. Add the 2 scenes to your build settings "Photon2Lobby" and "Photon2Room" in that order. If you want to use Hand tracking, instead of the "Photon2Room" add the "Photon2RoomHandTracking" scene in second place.
8. Enjoy :)

This template is a starting point for your project, it shows basic integration of Photon2 into Oculus integration, it is designed for oculus quest, but it is also compatible with oculus Rift/go.
The sample scene is configured for a max of 5 players (it can be changed in the NetworkManager script), and allows to talk and interact with an "ovr interactable" (for example you can give it to other player).
I use mostly "RPC" for sending information and events to the network, so I strongly suggest that you read the documentation here: https://doc.photonengine.com/en-us/pun/v2/gameplay/rpcsandraiseevent
And of course, I also suggest reading through the the rest of Photon documentation here: https://doc.photonengine.com/en-us/pun/v2/getting-started/pun-intro

Troubleshooting.

- If you are getting an error on building, with the file libyuv.a, find that file in the in your project and change the settings ticking off "Any platform".
- If when running the app it get stuck in "Connecting", check if your Oculus Quest have internet connection, and also check step 6, specially filling the appID in PhotonServerSettings.
- If you are getting connected, but can't see other players, try fixing your Region in PhotonServerSettings config file, you can find a list of regions in here: https://doc.photonengine.com/en-us/realtime/current/connection-and-authentication/regions

If you have any other trouble, don't hesitate to write: chiligamesco@gmail.com