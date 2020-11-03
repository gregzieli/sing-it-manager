import passport from "passport";
import { Strategy as LocalStrategy } from "passport-local";

import User from "../models/User";
import { verifyLogin, verifySignup } from "../middleware/auth";

passport.serializeUser((user, done) => {
    done(null, user.id);
});

passport.deserializeUser((id, done) => {
    User.findById(id, "-local.password", (error, user) => done(error, user));
});

passport.use(new LocalStrategy(verifyLogin));
passport.use("local-signup", new LocalStrategy(verifySignup));

export default passport;
