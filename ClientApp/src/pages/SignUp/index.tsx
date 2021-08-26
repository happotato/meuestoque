import * as React from "react";
import { Link } from "react-router-dom";
import { signUpActionAsync, useDispatch, useSelector } from "~/store";

export default function SignUp() {
  const dispatch = useDispatch();
  const { isLoadingUser, userErrorMsg } = useSelector((state) => state);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const data = new FormData(e.currentTarget);

    dispatch(
      signUpActionAsync({
        name: data.get("name") as string,
        username: data.get("username") as string,
        email: data.get("email") as string,
        password: data.get("password") as string,
      })
    );
  }

  return (
    <div className="vh-100 container d-flex justify-content-center align-items-center">
      <main className="col-xs-12 col-sm-7 col-md-5 center-block">
        <div className="text-center mb-5">
          <h1>
            <Link className="text-primary" to="/">
              <b>{"MeuEstoque"}</b>
            </Link>
          </h1>
        </div>
        <form onSubmit={onSubmit}>
          <div className="mb-3">
            <label htmlFor="textInput" className="form-label">
              {"Name"}
            </label>
            <input
              id="textInput"
              type="text"
              name="name"
              className="form-control"
              autoComplete="name"
            />
          </div>
          <div className="mb-3">
            <label htmlFor="usernameInput" className="form-label">
              {"Username"}
            </label>
            <input
              id="usernameInput"
              type="text"
              name="username"
              className="form-control"
              autoComplete="username"
              minLength={4}
              required
            />
          </div>
          <div className="mb-3">
            <label htmlFor="emailInput" className="form-label">
              {"Email"}
            </label>
            <input
              id="emailInput"
              type="email"
              name="email"
              className="form-control"
              autoComplete="email"
              required
            />
          </div>
          <div className="mb-3">
            <label htmlFor="passwordInput" className="form-label">
              {"Password"}
            </label>
            <input
              id="passwordInput"
              type="password"
              name="password"
              className="form-control"
              autoComplete="password"
              min={8}
              max={24}
              required
            />
          </div>
          <div className="w-100 mb-3">
            <button
              type="submit"
              className="btn btn-primary w-100"
              disabled={isLoadingUser}
            >
              {isLoadingUser && (
                <span
                  className="spinner-border spinner-border-sm me-2"
                  role="status"
                  aria-hidden="true"
                ></span>
              )}
              <span>{"Sign Up"}</span>
            </button>
          </div>
          {userErrorMsg && (
            <div className="text-center text-danger">
              <span>{userErrorMsg}</span>
            </div>
          )}
          <div className="text-center">
            <span>{"Already have an account? "}</span>
            <Link to="/login">{"Login"}</Link>
          </div>
        </form>
      </main>
    </div>
  );
}
