mergeInto(LibraryManager.library, {
    WebGLSave_Init: function () {
        if (window._fiSaveInitialized) return;
        window._fiSaveInitialized = true;

        var isIOS = /iPad|iPhone|iPod/.test(navigator.userAgent || "") ||
                    (navigator.platform === "MacIntel" && navigator.maxTouchPoints > 1);

        if (isIOS && window.self !== window.top) {
            try {
                window.top.location.replace(window.location.href);
                return;
            } catch (e) {}
        }

        var syncNow = function () {
            try {
                if (typeof FS !== "undefined" && FS.syncfs) {
                    FS.syncfs(false, function (err) {});
                }
            } catch (e) {}
        };

        window.addEventListener("pagehide", syncNow, false);
        window.addEventListener("beforeunload", syncNow, false);
        document.addEventListener("visibilitychange", function () {
            if (document.visibilityState === "hidden") {
                syncNow();
            }
        }, false);
    },

    WebGLSave_SetInt: function (keyPtr, val) {
        var k = "FI_" + UTF8ToString(keyPtr).replace(/[^a-zA-Z0-9_]/g, "_");
        var v = String(val | 0);
        try {
            window.localStorage.setItem(k, v);
        } catch (e) {}
        try {
            if (window.self === window.top) {
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=Lax; Secure";
            } else {
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=None; Secure; Partitioned";
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=None; Secure";
            }
        } catch (e) {}
    },

    WebGLSave_GetInt: function (keyPtr, defaultVal) {
        var k = "FI_" + UTF8ToString(keyPtr).replace(/[^a-zA-Z0-9_]/g, "_");
        try {
            var ls = window.localStorage.getItem(k);
            if (ls !== null && ls !== undefined && ls !== "") {
                var parsed = parseInt(ls, 10);
                if (!isNaN(parsed)) return parsed;
            }
        } catch (e) {}
        try {
            var cookies = document.cookie ? document.cookie.split("; ") : [];
            for (var i = 0; i < cookies.length; i++) {
                var parts = cookies[i].split("=");
                if (parts[0] === k && parts.length > 1) {
                    var cVal = parseInt(parts[1], 10);
                    if (!isNaN(cVal)) return cVal;
                }
            }
        } catch (e) {}
        return defaultVal;
    },

    WebGLSave_SetFloat: function (keyPtr, val) {
        var k = "FI_" + UTF8ToString(keyPtr).replace(/[^a-zA-Z0-9_]/g, "_");
        var v = String(val);
        try {
            window.localStorage.setItem(k, v);
        } catch (e) {}
        try {
            if (window.self === window.top) {
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=Lax; Secure";
            } else {
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=None; Secure; Partitioned";
                document.cookie = k + "=" + v + "; path=/; max-age=31536000; SameSite=None; Secure";
            }
        } catch (e) {}
    },

    WebGLSave_GetFloat: function (keyPtr, defaultVal) {
        var k = "FI_" + UTF8ToString(keyPtr).replace(/[^a-zA-Z0-9_]/g, "_");
        try {
            var ls = window.localStorage.getItem(k);
            if (ls !== null && ls !== undefined && ls !== "") {
                var parsed = parseFloat(ls);
                if (!isNaN(parsed)) return parsed;
            }
        } catch (e) {}
        try {
            var cookies = document.cookie ? document.cookie.split("; ") : [];
            for (var i = 0; i < cookies.length; i++) {
                var parts = cookies[i].split("=");
                if (parts[0] === k && parts.length > 1) {
                    var cVal = parseFloat(parts[1]);
                    if (!isNaN(cVal)) return cVal;
                }
            }
        } catch (e) {}
        return defaultVal;
    },

    WebGLSave_HasKey: function (keyPtr) {
        var k = "FI_" + UTF8ToString(keyPtr).replace(/[^a-zA-Z0-9_]/g, "_");
        try {
            if (window.localStorage.getItem(k) !== null) return 1;
        } catch (e) {}
        try {
            var cookies = document.cookie ? document.cookie.split("; ") : [];
            for (var i = 0; i < cookies.length; i++) {
                var parts = cookies[i].split("=");
                if (parts[0] === k) return 1;
            }
        } catch (e) {}
        return 0;
    },

    WebGLSave_DeleteAll: function () {
        try {
            var keysToRemove = [];
            for (var i = 0; i < window.localStorage.length; i++) {
                var key = window.localStorage.key(i);
                if (key && key.indexOf("FI_") === 0) {
                    keysToRemove.push(key);
                }
            }
            for (var j = 0; j < keysToRemove.length; j++) {
                window.localStorage.removeItem(keysToRemove[j]);
            }
        } catch (e) {}
        try {
            var cookies = document.cookie ? document.cookie.split("; ") : [];
            for (var c = 0; c < cookies.length; c++) {
                var name = cookies[c].split("=")[0];
                if (name && name.indexOf("FI_") === 0) {
                    document.cookie = name + "=; path=/; max-age=0; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Lax; Secure";
                    document.cookie = name + "=; path=/; max-age=0; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=None; Secure; Partitioned";
                    document.cookie = name + "=; path=/; max-age=0; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=None; Secure";
                    document.cookie = name + "=; path=/; max-age=0; expires=Thu, 01 Jan 1970 00:00:00 GMT";
                }
            }
        } catch (e) {}
    },

    WebGLSave_Sync: function () {
        try {
            if (window._fiSyncTimer) clearTimeout(window._fiSyncTimer);
            window._fiSyncTimer = setTimeout(function () {
                try {
                    if (typeof FS !== "undefined" && FS.syncfs) {
                        FS.syncfs(false, function (err) {});
                    }
                } catch (e) {}
            }, 150);
        } catch (e) {}
    }
});
