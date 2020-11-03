import User from "../models/User";

const verifyLogin = async (username, password, done) => {
    try {
        const user = await User.findOne({ "local.username": username });

        if (!user) {
            return done(null, false, { message: "Incorrect username." });
        }

        if (!(await user.verifyPassword(password))) {
            return done(null, false, { message: "Incorrect password." });
        }

        return done(null, user);
    } catch (error) {
        done(error);
    }
};

const verifySignup = async (username, password, done) => {
    try {
        const dbUser = await User.findOne({ "local.username": username });

        if (dbUser) {
            return done(null, false, { msg: "User already exists." });
        }

        const user = new User({ local: { username, password } });
        await user.save();

        return done(null, user);
    } catch (error) {
        done(error);
    }
};

const authorize = (req, res, next) => {
    if (req.isAuthenticated()) {
        return next();
    }

    res.send(
        "Session authorization required. This would normally be a redirect to login page."
    );
};

export default authorize;
export { verifyLogin, verifySignup };
