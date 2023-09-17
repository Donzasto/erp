<script lang="ts">
	import { onMount } from 'svelte';

	let login: string;
	let password: string;

	onMount(async () => {
		getUsers();
	});

	let users: User[] = [];

	async function getUsers() {
		const response = await fetch('/api/users', {
			method: 'GET',
			headers: { Accept: 'application/json' }
		});

		if (response.ok === true) {
			users = await response.json();
		}
	}

	async function addUser() {
		const response = await fetch('api/users', {
			method: 'POST',
			headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
			body: JSON.stringify({
				login: login,
				password: password
			})
		});
		if (response.ok === true) {
			const user = await response.json();
			// document.querySelector(`tr[data-rowid='${user.id}']`)!.replaceWith(row(user));
		} else {
			const error = await response.json();
			console.log(error.message);
		}
	}

	async function editUser(id: number) {
		const response = await fetch('api/users', {
			method: 'PUT',
			headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
			body: JSON.stringify({
				id: id,
				login: login,
				password: password
			})
		});
		if (response.ok === true) {
			const user = await response.json();
			// document.querySelector(`tr[data-rowid='${user.id}']`)!.replaceWith(row(user));
		} else {
			const error = await response.json();
			console.log(error.message);
		}
	}

	async function deleteUser(id: number) {
		const response = await fetch(`/api/users/${id}`, {
			method: 'DELETE',
			headers: { Accept: 'application/json' }
		});
		if (response.ok === true) {
			const user = await response.json();
			document.querySelector(`tr[data-rowid='${user.id}']`)!.remove();
		} else {
			const error = await response.json();
			console.log(error.message);
		}
	}
</script>

<div class="parent">
	<div>
		<table>
			<thead>
				<tr>
					<th>Login</th>
					<th>Password</th>
				</tr>
			</thead>
			<tbody>
				{#each users as user}
					<tr>
						<td>{user.login}</td>
						<td>{user.password}</td>
						<td><input type="submit" value="Edit" on:click={() => editUser(user.id)} /> </td>
						<td><input type="submit" value="Delete" on:click={() => deleteUser(user.id)} /></td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>
	<div>
		<p>
			<label for="login">Login</label><br />
			<input name="login" bind:value={login} />
		</p>
		<p>
			<label for="password">Password</label><br />
			<input name="password" bind:value={password} />
		</p>
		<input type="submit" value="Save" on:click={addUser} />
	</div>
</div>

<style>
	.parent {
		display: grid;
		grid-template-columns: repeat(2, 1fr);
	}
</style>
