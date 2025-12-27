// WeChatStorage.jslib
mergeInto(LibraryManager.library, {
  // 保存游戏数据到微信存储
  // jsonStringPtr 是 C# 传递过来的 JSON 字符串的指针
  SaveGameDataToWeChat: function (jsonStringPtr) {
    try {
      // 将 C# 传递过来的 UTF8 字符串指针转换为 JavaScript 字符串
      var jsonString = UTF8ToString(jsonStringPtr);
      var data = JSON.parse(jsonString); // 解析 JSON 字符串为 JavaScript 对象
      wx.setStorageSync('game_save_data', data); // 调用微信小程序的同步存储 API
      console.log("游戏数据已保存到微信存储");
    } catch (e) {
      console.error("保存数据失败: ", e);
    }
  },

  // 从微信存储加载游戏数据
  // 返回一个指针，指向 Emscripten 堆上的 UTF8 编码的 JSON 字符串
  LoadGameDataFromWeChat: function () {
    try {
      var data = wx.getStorageSync('game_save_data'); // 从微信小程序的本地存储读取数据
      // 如果有数据，将其转换为 JSON 字符串；否则返回空字符串
      var resultJson = data ? JSON.stringify(data) : "";

      // 将 JavaScript 字符串转换为 Emscripten 堆上的 UTF8 字符串
      // _malloc 分配内存，stringToUTF8 写入数据
      var bufferSize = lengthBytesUTF8(resultJson) + 1; // +1 是为了 null 终止符
      var buffer = _malloc(bufferSize);
      stringToUTF8(resultJson, buffer, bufferSize);
      return buffer; // 返回内存地址的指针给 C#
    } catch (e) {
      console.error("加载数据失败: ", e);
      // 发生错误时也返回空字符串，确保 C# 端能处理
      var errorJson = "";
      var bufferSize = lengthBytesUTF8(errorJson) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(errorJson, buffer, bufferSize);
      return buffer;
    }
  },
});
