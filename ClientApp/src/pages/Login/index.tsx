import * as React from "react";
import { Link } from "react-router-dom";
import { logInActionAsync, useDispatch, useSelector } from "~/store";

export default function Login() {
  const dispatch = useDispatch();
  const { isLoadingUser, userErrorMsg } = useSelector((state) => state);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const data = new FormData(e.currentTarget);
    const email = data.get("email") as string;
    const password = data.get("password") as string;

    dispatch(logInActionAsync(email, password));
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
              <span>{"Login"}</span>
            </button>
          </div>
          {userErrorMsg && (
            <div className="text-center text-danger">
              <span>{userErrorMsg}</span>
            </div>
          )}
          <div className="text-center">
            <span>{"Don't have an account? "}</span>
            <Link to="/signup">{"Join"}</Link>
          </div>
        </form>
      </main>
    </div>
  );
}
