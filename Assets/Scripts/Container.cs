public sealed class Container {
    // -- props --
    public Player player;

    // -- singleton --
    private static readonly Container instance = new Container();

    public static Container Get() {
        return instance;
    }
}
