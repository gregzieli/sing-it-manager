import mongoose from "mongoose";
import bcrypt from "bcryptjs";

const UserSchema = new mongoose.Schema(
    {
        local: {
            username: {
                type: String,
                unique: true,
                required: true,
            },
            password: {
                type: String,
                required: true,
            },
        },
    },
    { timestamps: true }
);

UserSchema.pre("save", async function (next) {
    if (this.isModified("local.password")) {
        this.local.password = await bcrypt.hash(this.local.password, 10);
    }
    return next();
});

UserSchema.methods.verifyPassword = async function (password) {
    return await bcrypt.compare(password, this.local.password);
};

export default mongoose.model("user", UserSchema);
