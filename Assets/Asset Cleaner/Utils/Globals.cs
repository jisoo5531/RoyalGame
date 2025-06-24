namespace Asset_Cleaner {
    static class Globals<T> where T : class {
        static T _Instance;

        public static T Value {
            get {
                Asr.IsFalse(_Instance == null);
                return _Instance;
            }
            set {
                var was = HasValue();
                _Instance = value;

                // keep counter to check during deinitialization if all Globals are cleared     
                if (was && !HasValue())
                    __GlobalsCounter.Counter -= 1;
                if (!was && HasValue())
                    __GlobalsCounter.Counter += 1;

                bool HasValue() => _Instance != null;
            }
        }
    }

    static class __GlobalsCounter {
        internal static int Counter;
        public static bool HasAnyValue() => Counter > 0;
    }
}